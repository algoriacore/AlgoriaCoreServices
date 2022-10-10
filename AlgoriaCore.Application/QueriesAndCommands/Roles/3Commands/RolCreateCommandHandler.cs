using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.Managers.Roles.Dto;
using AlgoriaCore.Domain.Authorization;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands
{
    public class RolCreateCommandHandler : BaseCoreClass, IRequestHandler<RolCreateCommand, long>
    {
        private readonly RolManager _rolManager;
        private readonly IAppAuthorizationProvider _authorizationProvider;

        public RolCreateCommandHandler(ICoreServices coreServices, RolManager rolManager, IAppAuthorizationProvider authorizationProvider) : base(coreServices)
        {
            _rolManager = rolManager;
            _authorizationProvider = authorizationProvider;
        }

        public async Task<long> Handle(RolCreateCommand request, CancellationToken cancellationToken)
        {
            var rolDto = new RolDto
            {
                TenantId = SessionContext.TenantId,
                Name = request.Name,
                DisplayName = request.DisplayName,
                IsActive = request.IsActive
            };

            var permisoList = request.GrantedPermissionNames ?? new List<string>();
            var pNames = _authorizationProvider.GetPermissionsFromNamesByValidating(permisoList);

            rolDto.Id = await _rolManager.AddRolAsync(rolDto, pNames.Select(m => m.DisplayName).ToList());

            _authorizationProvider.GetPermissionsFromNamesByValidating(permisoList);
            var permisoDtoList = permisoList.Select(s => new PermissionDto { Name = s }).ToList();
            await _rolManager.ReplacePermissionAsync(rolDto.Id.Value, permisoDtoList);

            return rolDto.Id.Value;
        }
    }
}
