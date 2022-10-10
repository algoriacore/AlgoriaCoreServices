using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Application.Managers.Tenants.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants._3Commands
{
    public class UpdateTenantCommandHandler : BaseCoreClass, IRequestHandler<UpdateTenantCommand, int>
    {
        private readonly TenantManager _tenantManager;

        public UpdateTenantCommandHandler(ICoreServices coreServices, TenantManager tenantManager) 
            : base(coreServices)
        {
            _tenantManager = tenantManager;
        }

        public async Task<int> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            TenantDto dto = new TenantDto()
            {
                Id = request.Id,
                TenancyName = request.TenancyName,
                Name = request.Name,
                IsActive = request.IsActive
            };

            await _tenantManager.UpdateTenantAsync(dto);

            return dto.Id;
        }
    }
}
