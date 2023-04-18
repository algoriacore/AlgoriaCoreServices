using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensiones;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails.Dto;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Application.Managers.Tenants.Dto;
using AlgoriaCore.Domain.Excel;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.PDF;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMails
{
    public class MailServiceMailExportPDFQueryHandler : BaseCoreClass, IRequestHandler<MailServiceMailExportPDFQuery, FileDto>
    {
        private readonly MailServiceMailManager _manager;
        private readonly TenantManager _managerTenant;

        private readonly IPDFService _pdfService;

        public MailServiceMailExportPDFQueryHandler(
            ICoreServices coreServices,
            MailServiceMailManager manager,
            TenantManager managerTenant,
            IPDFService pdfService) : base(coreServices)
        {
            _manager = manager;
            _managerTenant = managerTenant;

            _pdfService = pdfService;
        }

        public async Task<FileDto> Handle(MailServiceMailExportPDFQuery request, CancellationToken cancellationToken)
        {
            var filterDto = new MailServiceMailListFilterDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                IsPaged = request.IsPaged,

                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TenantId = request.TenantId,
                OnlyHost = request.OnlyHost
            };

            PagedResultDto<MailServiceMailDto> pagedResultDto = null;

            if (SessionContext.TenantId.HasValue)
            {
                pagedResultDto = await _manager.GetMailServiceMailPagedListAsync(filterDto);
            }
            else
            {
                pagedResultDto = await _manager.GetMailServiceMailPagedListByHostAsync(filterDto);
            }

            List<ExpandoObject> ll = new List<ExpandoObject>();
            dynamic l;

            foreach (var item in pagedResultDto.Items)
            {
                l = new ExpandoObject();

                l.Id = item.Id;
                l.MailServiceRequest = item.MailServiceRequest;
                l.MailServiceRequestDate = item.MailServiceRequestDate.Value.ToZone(SessionContext.TimeZone).ToString("dd/MM/yyyy HH:mm:ss");
                l.IsLocalConfig = item.IsLocalConfig;
                l.IsLocalConfigDesc = item.IsLocalConfigDesc;
                l.Sendto = item.Sendto;
                l.CopyTo = item.CopyTo;
                l.Subject = item.Subject;
                l.Status = item.Status;
                l.StatusDesc = item.StatusDesc;

                ll.Add(l);
            }

            List<IViewColumn> columns = JsonConvert.DeserializeObject<List<ViewColumn>>(request.ViewColumnsConfigJSON)
                .Cast<IViewColumn>().ToList();

            byte[] bytes = await _pdfService.ExportView(L("MailServiceMails.MailServiceMail"), ll, columns, await GetViewFilters(request));
            return new FileDto
            {
                FileName = "ViewMailServiceMails.pdf",
                FileType = "PDF",
                FileBase64 = Convert.ToBase64String(bytes)
            };
        }

        private async Task<List<IViewFilter>> GetViewFilters(MailServiceMailExportPDFQuery query)
        {
            List<IViewFilter> filters = new List<IViewFilter>();
            string dateRangeStr = (query.StartDate.HasValue ? query.StartDate.Value.ToShortDateString() : "") + "-"
                + (query.EndDate.HasValue ? query.EndDate.Value.ToShortDateString() : "");
            TenantDto tenantDto = null;

            if (query.TenantId.HasValue)
            {
                tenantDto = await _managerTenant.GetTenantByIdAsync(query.TenantId.Value);
            }

            filters.Add(new ViewFilter { Name = "DateRange", Title = L("MailServiceMails.MailServiceMail.DateRange"), Value = dateRangeStr });
            filters.Add(new ViewFilter { Name = nameof(query.TenantId), Title = L("MailServiceMails.MailServiceMail.Tenant"), Value = tenantDto == null ? "" : tenantDto.TenancyName + " - " + tenantDto.Name });
            filters.Add(new ViewFilter { Name = nameof(query.OnlyHost), Title = L("MailServiceMails.MailServiceMail.OnlyHost"), Value = query.OnlyHost == true ? L("Yes") : L("No") });
            filters.Add(new ViewFilter { Name = nameof(query.Filter), Title = L("SearchDots"), Value = query.Filter });

            return filters;
        }
    }
}
