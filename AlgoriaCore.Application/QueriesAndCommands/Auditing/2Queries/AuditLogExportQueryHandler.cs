using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensiones;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Auditing;
using AlgoriaCore.Application.Managers.Auditing.Dto;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Application.Managers.Tenants.Dto;
using AlgoriaCore.Domain.Excel;
using AlgoriaCore.Domain.Interfaces.Excel;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Auditing
{
    public class AuditLogExportQueryHandler : BaseCoreClass, IRequestHandler<AuditLogExportQuery, FileDto>
    {
        private readonly AuditLogManager _manager;
        private readonly TenantManager _managerTenant;

        private readonly IExcelService _excelService;

        public AuditLogExportQueryHandler(
            ICoreServices coreServices,
            AuditLogManager manager,
            TenantManager managerTenant,
            IExcelService excelService) : base(coreServices)
        {
            _manager = manager;
            _managerTenant = managerTenant;

            _excelService = excelService;
        }

        public async Task<FileDto> Handle(AuditLogExportQuery request, CancellationToken cancellationToken)
        {
            var filter = new AuditLogListFilterDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                IsPaged = request.IsPaged,

                StartDate = request.StartDate,
                EndDate = request.EndDate,
                UserName = request.UserName,
                ServiceName = request.ServiceName,
                MethodName = request.MethodName,
                BrowserInfo = request.BrowserInfo,
                TenantId = request.TenantId,
                HasException = request.HasException,
                MinExecutionDuration = request.MinExecutionDuration,
                MaxExecutionDuration = request.MaxExecutionDuration,
                Severity = request.Severity,
                OnlyHost = request.OnlyHost
            };

            PagedResultDto<AuditLogDto> pagedResultDto = null;

            if (SessionContext.TenantId.HasValue)
            {
                pagedResultDto = await _manager.GetAuditLogList(filter);
            }
            else
            {
                pagedResultDto = await _manager.GetAuditLogByHostList(filter);
            }

            List<ExpandoObject> ll = new List<ExpandoObject>();
            dynamic l;

            foreach (var item in pagedResultDto.Items)
            {
                l = new ExpandoObject();

                l.Id = item.Id.Value;
                l.TenantName = item.TenantName;
                l.UserId = item.UserId;
                l.UserName = item.UserName;
                l.ImpersonalizerUserId = item.ImpersonalizerUserId;
                l.ImpersonalizerUserName = item.ImpersonalizerUserName;
                l.ServiceName = item.ServiceName;
                l.MethodName = item.MethodName;
                l.Parameters = item.Parameters;
                l.ExecutionTime = item.ExecutionTime.Value.ToZone(SessionContext.TimeZone).ToString("F"); // .ToString("ddd MMM dd yyyy HH: mm: ss Z")
                l.ExecutionDuration = item.ExecutionDuration;
                l.ClientIpAddress = item.ClientIpAddress;
                l.ClientName = item.ClientName;
                l.BrowserInfo = item.BrowserInfo;
                l.Exception = item.Exception;
                l.CustomData = item.CustomData;
                l.Severity = item.Severity;

                ll.Add(l);
            }

            List<IViewColumn> columns = JsonConvert.DeserializeObject<List<ViewColumn>>(request.ViewColumnsConfigJSON)
                .Cast<IViewColumn>().ToList();

            var file = _excelService.ExportView(L("AuditLogs"), "ViewAuditLogs", ll, columns, await GetViewFilters(request));

            return new FileDto
            {
                FileName = file.FileName,
                FileToken = file.FileToken,
                FileType = file.FileType,
                FileBase64 = Convert.ToBase64String(file.FileArray)
            };
        }

        private async Task<List<IViewFilter>> GetViewFilters(AuditLogExportQuery query)
        {
            List<IViewFilter> filters = new List<IViewFilter>();
            string dateRangeStr = query.StartDate.ToZone(SessionContext.TimeZone).ToShortDateString() + "-" + query.EndDate.ToZone(SessionContext.TimeZone).ToShortDateString();
            string durationStr = (query.MinExecutionDuration.HasValue ? query.MinExecutionDuration.Value.ToString("#,###") : "")
                + "-" + (query.MaxExecutionDuration.HasValue ? query.MaxExecutionDuration.Value.ToString("#,###") : "");
            TenantDto tenantDto = null;

            if (query.TenantId.HasValue)
            {
                tenantDto = await _managerTenant.GetTenantByIdAsync(query.TenantId.Value);
            }

            string severityStr = L("AuditLogs.All");

            switch(query.Severity)
            {
                case 0:
                    severityStr = L("LogLevelTrace");
                    break;
                case 1:
                    severityStr = L("LogLevelDebug");
                    break;
                case 2:
                    severityStr = L("LogLevelInformation");
                    break;
                case 3:
                    severityStr = L("LogLevelWarning");
                    break;
                case 4:
                    severityStr = L("LogLevelError");
                    break;
                case 5:
                    severityStr = L("LogLevelCritical");
                    break;
            }

            filters.Add(new ViewFilter { Name = "DateRange", Title = L("MailServiceMails.MailServiceMail.DateRange"), Value = dateRangeStr });
            filters.Add(new ViewFilter { Name = nameof(query.UserName), Title = L("AuditLogs.UserName"), Value = query.UserName });
            filters.Add(new ViewFilter { Name = nameof(query.Severity), Title = L("AuditLogs.ErrorState"), Value = severityStr });
            filters.Add(new ViewFilter { Name = "Duration", Title = L("AuditLogs.Duration"), Value = durationStr });
            filters.Add(new ViewFilter { Name = nameof(query.ServiceName), Title = L("AuditLogs.Service"), Value = query.ServiceName });
            filters.Add(new ViewFilter { Name = nameof(query.MethodName), Title = L("AuditLogs.Action"), Value = query.MethodName });
            filters.Add(new ViewFilter { Name = nameof(query.BrowserInfo), Title = L("AuditLogs.Browser"), Value = query.BrowserInfo });
            filters.Add(new ViewFilter { Name = nameof(query.TenantId), Title = L("AuditLogs.Tenant"), Value = tenantDto == null ? "" : tenantDto.TenancyName + " - " + tenantDto.Name });
            filters.Add(new ViewFilter { Name = nameof(query.OnlyHost), Title = L("AuditLogs.OnlyHost"), Value = query.OnlyHost == true ? L("Yes") : L("No") });
            //filters.Add(new ViewFilter { Name = nameof(query.Filter), Title = L("SearchDots"), Value = query.Filter });

            return filters;
        }
    }
}
