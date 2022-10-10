using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Tenants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants._2Queries
{

    public class GetTenantQueryHandler : BaseCoreClass, IRequestHandler<GetTenantQuery, TenantResponse>
    {
        private readonly TenantManager _tenantManager;

        public GetTenantQueryHandler(TenantManager tenantManager, ICoreServices coreServices)
                                : base(coreServices)
        {
            _tenantManager = tenantManager;
        }

        public async Task<TenantResponse> Handle(GetTenantQuery request, CancellationToken cancellationToken)
        {
            var dto = await _tenantManager.GetTenantByIdAsync(request.Id);

            var tR = new TenantResponse {
                Id = dto.Id,
                TenancyName = dto.TenancyName,
                Name = dto.Name,
                CreationTime = dto.CreationTime,
                IsActive = dto.IsActive,
                IsDeleted = dto.IsDeleted
            };

            return tR;
        }
    }
}
