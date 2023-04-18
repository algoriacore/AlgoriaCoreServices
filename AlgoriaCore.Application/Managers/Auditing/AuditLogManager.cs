using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Auditing.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Auditing
{
    public class AuditLogManager : BaseManager
    {
        private readonly IRepository<AuditLog, long> _repAuditLog;
        private readonly IRepository<User, long> _repUser;
        private readonly IRepository<Tenant, int> _repTenant;

        public AuditLogManager(
            IRepository<AuditLog, long> repAuditLog,
            IRepository<User, long> repUser,
            IRepository<Tenant, int> repTenant)
        {
            _repAuditLog = repAuditLog;
            _repUser = repUser;
            _repTenant = repTenant;
        }

        public async Task<PagedResultDto<AuditLogDto>> GetAuditLogByHostList(AuditLogListFilterDto input)
        {
			List<AuditLogDto> results;
            int count = 0;

            using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
            {
                using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                {
                    var query = CreateAuditLogAndUsersQuery(input);

                    if (input.OnlyHost == true)
                    {
                        query = query.Where(m => m.TenantId == null);
                    }
                    else if(input.TenantId.HasValue)
                    {
                        query = query.Where(m => m.TenantId == input.TenantId);
                    }

                    count = await query.CountAsync();
                    results = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "ExecutionTime DESC" : input.Sorting)
                        .PageByIf(input.IsPaged, input)
                        .ToListAsync();
                }
            }

            return new PagedResultDto<AuditLogDto>(count, results);
        }

        public async Task<PagedResultDto<AuditLogDto>> GetAuditLogList(AuditLogListFilterDto input)
        {
            return await DoGetAuditLogList(input);
        }

        private async Task<PagedResultDto<AuditLogDto>> DoGetAuditLogList(AuditLogListFilterDto input)
        {
            if (CurrentUnitOfWork.GetTenantId() != null) {
                input.TenantId = CurrentUnitOfWork.GetTenantId();
            }

			List<AuditLogDto> results;
            int count = 0;

            using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
            {
                using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                {
                    var query = CreateAuditLogAndUsersQuery(input)
                        .WhereIf(input.TenantId.HasValue, item => item.TenantId == input.TenantId);

                    count = await query.CountAsync();
                    results = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "ExecutionTime DESC" : input.Sorting)
                        .PageByIf(input.IsPaged, input)
                        .ToListAsync();
                }
            }

            return new PagedResultDto<AuditLogDto>(count, results);
        }

        private IQueryable<AuditLogDto> CreateAuditLogAndUsersQuery(AuditLogListFilterDto input)
        {
            var _namespaceStripper = new NamespaceStripper();

            var query = (from auditLog in _repAuditLog.GetAll()
                         join tenant in _repTenant.GetAll() on auditLog.TenantId equals tenant.Id into tenantJoin
                         from joinedTenant in tenantJoin.DefaultIfEmpty()
                         join user in _repUser.GetAll() on auditLog.UserId equals user.Id into userJoin
                         from joinedUser in userJoin.DefaultIfEmpty()
                         join impersonalizer in _repUser.GetAll() on auditLog.ImpersonalizerUserId equals impersonalizer.Id into impersonalizerJoin
                         from joinedImpersonalizer in impersonalizerJoin.DefaultIfEmpty()
                         where auditLog.ExecutionDatetime >= input.StartDate && auditLog.ExecutionDatetime <= input.EndDate
                         && (input.ServiceName.IsNullOrWhiteSpace() || auditLog.ServiceName.Contains(input.ServiceName))
                         //&& auditLog.TenantId == input.TenantId
                         select new AuditLogDto {
                          Id = auditLog.Id,
                          TenantId = auditLog.TenantId,
                          TenantName = joinedTenant != null ? joinedTenant.Name : null,
                          UserId = auditLog.UserId,
                          UserName = joinedUser != null ? joinedUser.UserLogin : null,
                          ImpersonalizerUserId = auditLog.ImpersonalizerUserId,
                          ImpersonalizerUserName = joinedImpersonalizer.UserLogin,
                          ServiceName = _namespaceStripper.StripNameSpace(auditLog.ServiceName),
                          MethodName = auditLog.MethodName,
                          Parameters = auditLog.Parameters,
                          ExecutionTime = auditLog.ExecutionDatetime,
                          ExecutionDuration = auditLog.ExecutionDuration,
                          ClientIpAddress = auditLog.ClientIpAddress,
                          ClientName = auditLog.ClientName,
                          BrowserInfo = auditLog.BroserInfo,
                          Exception = auditLog.Exception,
                          CustomData = auditLog.CustomData,
                          Severity = auditLog.Severity
                      });

            query = query
                .WhereIf(!input.UserName.IsNullOrWhiteSpace(), item => item.UserName.Contains(input.UserName))
                //.WhereIf(!input.ServiceName.IsNullOrWhiteSpace(), item => item.ServiceName.Contains(input.ServiceName))
                .WhereIf(!input.MethodName.IsNullOrWhiteSpace(), item => item.MethodName.Contains(input.MethodName))
                .WhereIf(!input.BrowserInfo.IsNullOrWhiteSpace(), item => item.BrowserInfo.Contains(input.BrowserInfo))
                .WhereIf(input.MinExecutionDuration.HasValue && input.MinExecutionDuration > 0, item => item.ExecutionDuration >= input.MinExecutionDuration.Value)
                .WhereIf(input.MaxExecutionDuration.HasValue && input.MaxExecutionDuration < int.MaxValue, item => item.ExecutionDuration <= input.MaxExecutionDuration.Value)
                .WhereIf(input.HasException == true, item => item.Exception != null && item.Exception != "")
                .WhereIf(input.HasException == false, item => item.Exception == null || item.Exception == "")
                .WhereIf(input.Severity != null, item => item.Severity == input.Severity);

            return query;
        }
    }
}
