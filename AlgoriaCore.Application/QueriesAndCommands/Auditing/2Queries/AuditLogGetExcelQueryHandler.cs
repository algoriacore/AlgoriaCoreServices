using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensiones;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Auditing;
using AlgoriaCore.Application.Managers.Auditing.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Auditing._1Model;
using AlgoriaCore.Domain.Excel.Auditing;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.Excel.Auditing;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Auditing._2Queries
{
    public class AuditLogGetExcelQueryHandler : BaseCoreClass, IRequestHandler<AuditLogGetExcelQuery, AuditLogExcelResponse>
    {
        private readonly AuditLogManager _manager;
        private readonly IExcelService _excelService;

        public AuditLogGetExcelQueryHandler(ICoreServices coreServices, AuditLogManager manager, IExcelService excelService) : base(coreServices)
        {
            _manager = manager;
            _excelService = excelService;
        }

        public async Task<AuditLogExcelResponse> Handle(AuditLogGetExcelQuery request, CancellationToken cancellationToken)
        {
            var filter = new AuditLogListFilterDto
            {
                Filter = request.Filter,
                PageNumber = 1,
                PageSize = 999999,
                Sorting = request.Sorting,

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
            var ll = new List<IAuditLogExcel>();

            if (SessionContext.TenantId.HasValue)
            {
                pagedResultDto = await _manager.GetAuditLogList(filter);
            }
            else
            {
                pagedResultDto = await _manager.GetAuditLogByHostList(filter);
            }

            foreach (var item in pagedResultDto.Items)
            {
                ll.Add(new AuditLogExcel
                {
                    Id = item.Id.Value,
                    //TenantId = item.TenantId,
                    TenantName = item.TenantName,
                    UserId = item.UserId,
                    UserName = item.UserName,
                    ImpersonalizerUserId = item.ImpersonalizerUserId,
                    ImpersonalizerUserName = item.ImpersonalizerUserName,
                    ServiceName = item.ServiceName,
                    MethodName = item.MethodName,
                    Parameters = item.Parameters,
                    ExecutionTime = item.ExecutionTime.Value.ToZone(SessionContext.TimeZone),
                    ExecutionDuration = item.ExecutionDuration,
                    ClientIpAddress = item.ClientIpAddress,
                    ClientName = item.ClientName,
                    BrowserInfo = item.BrowserInfo,
                    Exception = item.Exception,
                    CustomData = item.CustomData,
					Severity = item.Severity
                });
            }

            var filterXLS = new AuditLogFilterExcel
            {
                StartDate = filter.StartDate.ToZone(SessionContext.TimeZone),
                EndDate = filter.EndDate.ToZone(SessionContext.TimeZone),
                UserName = filter.UserName,
                ServiceName = filter.ServiceName,
                MethodName = filter.MethodName,
                BrowserInfo = filter.BrowserInfo,
                //TenantId = filter.TenantId,
                HasException = filter.HasException,
                MinExecutionDuration = filter.MinExecutionDuration,
                MaxExecutionDuration = filter.MaxExecutionDuration,
                Severity = filter.Severity
            };

            var file = _excelService.ExportAuditLogsToFile(filterXLS, ll);

            return await Task.FromResult(new AuditLogExcelResponse
            {
                FileName = file.FileName,
                FileToken = file.FileToken,
                FileType = file.FileType
            });
        }
    }
}
