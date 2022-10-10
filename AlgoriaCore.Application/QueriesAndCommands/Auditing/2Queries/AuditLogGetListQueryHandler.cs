using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Auditing;
using AlgoriaCore.Application.Managers.Auditing.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Auditing._1Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Auditing._2Queries
{
    public class AuditLogGetListQueryHandler : BaseCoreClass, IRequestHandler<AuditLogGetListQuery, PagedResultDto<AuditLogListResponse>>
    {
        private readonly AuditLogManager _manager;

        public AuditLogGetListQueryHandler(ICoreServices coreServices, AuditLogManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<AuditLogListResponse>> Handle(AuditLogGetListQuery request, CancellationToken cancellationToken)
        {
            var filter = new AuditLogListFilterDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
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
            var ll = new List<AuditLogListResponse>();

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
                ll.Add(new AuditLogListResponse
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
                    ExecutionTime = item.ExecutionTime,
                    ExecutionDuration = item.ExecutionDuration,
                    ClientIpAddress = item.ClientIpAddress,
                    ClientName = item.ClientName,
                    BrowserInfo = item.BrowserInfo,
                    Exception = item.Exception,
                    CustomData = item.CustomData,
                    Severity = item.Severity
                });
            }

            return new PagedResultDto<AuditLogListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
