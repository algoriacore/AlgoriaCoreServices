using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensiones;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Auditing;
using AlgoriaCore.Application.Managers.Auditing.Dto;
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

namespace AlgoriaCore.Application.QueriesAndCommands.Auditing
{
    public class AuditLogExportCSVQueryHandler : BaseCoreClass, IRequestHandler<AuditLogExportCSVQuery, FileDto>
    {
        private readonly AuditLogManager _manager;

        private readonly ICSVService _csvService;

        public AuditLogExportCSVQueryHandler(ICoreServices coreServices, AuditLogManager manager, ICSVService csvService) : base(coreServices)
        {
            _manager = manager;

            _csvService = csvService;
        }

        public async Task<FileDto> Handle(AuditLogExportCSVQuery request, CancellationToken cancellationToken)
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

            byte[] bytes = _csvService.ExportView(ll, columns);

            return new FileDto
            {
                FileName = "ViewAuditLogs.csv",
                FileType = "CSV",
                FileBase64 = Convert.ToBase64String(bytes)
            };
        }
    }
}
