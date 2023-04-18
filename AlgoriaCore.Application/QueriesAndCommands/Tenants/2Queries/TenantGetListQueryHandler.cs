using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Application.QueriesAndCommands.Tenants._1Model;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants
{
    public class TenantGetListQueryHandler : IRequestHandler<TenantGetListQuery, PagedResultDto<TenantListResponse>>
    {
        private readonly TenantManager _tenantManager;

        public TenantGetListQueryHandler(TenantManager tenantManager,
                                ILogger<TenantGetListQueryHandler> logger)
        {
            _tenantManager = tenantManager;
        }

        public async Task<PagedResultDto<TenantListResponse>> Handle(TenantGetListQuery request, CancellationToken cancellationToken)
        {
            var filter = new PageListByDto {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            var pagedResultDto = await _tenantManager.GetTenantsListAsync(filter);
            var ll = new List<TenantListResponse>();

            foreach (var item in pagedResultDto.Items)
            {
                ll.Add(new TenantListResponse {
                    Id = item.Id,
                    Name = item.Name,
                    TenancyName = item.TenancyName,
                    IsActive = item.IsActive,
                    IsActiveDesc = item.IsActiveDesc,
                    CreationTime = item.CreationTime
                });
            }

            return new PagedResultDto<TenantListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
