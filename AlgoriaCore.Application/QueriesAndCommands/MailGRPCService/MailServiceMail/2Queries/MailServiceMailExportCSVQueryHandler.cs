using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensiones;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails.Dto;
using AlgoriaCore.Domain.Excel;
using AlgoriaCore.Domain.Interfaces.CSV;
using AlgoriaCore.Domain.Interfaces.Excel;
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
    public class RoleExportCSVQueryHandler : BaseCoreClass, IRequestHandler<MailServiceMailExportCSVQuery, FileDto>
    {
        private readonly MailServiceMailManager _manager;

        private readonly ICSVService _csvService;

        public RoleExportCSVQueryHandler(ICoreServices coreServices, MailServiceMailManager manager, ICSVService csvService) : base(coreServices)
        {
            _manager = manager;

            _csvService = csvService;
        }

        public async Task<FileDto> Handle(MailServiceMailExportCSVQuery request, CancellationToken cancellationToken)
        {
            MailServiceMailListFilterDto filterDto = new MailServiceMailListFilterDto
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

            byte[] bytes = _csvService.ExportView(ll, columns);

            return new FileDto
            {
                FileName = "ViewMailServiceMails.csv",
                FileType = "CSV",
                FileBase64 = Convert.ToBase64String(bytes)
            };
        }
    }
}
