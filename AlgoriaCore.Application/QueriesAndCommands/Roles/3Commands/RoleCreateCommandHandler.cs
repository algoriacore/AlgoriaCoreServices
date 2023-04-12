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
    public class RoleCreateCommandHandler : BaseCoreClass, IRequestHandler<RoleCreateCommand, long>
    {
        private readonly RoleManager _roleManager;
        private readonly IAppAuthorizationProvider _authorizationProvider;

        public RoleCreateCommandHandler(ICoreServices coreServices, RoleManager roleManager, IAppAuthorizationProvider authorizationProvider) : base(coreServices)
        {
            _roleManager = roleManager;
            _authorizationProvider = authorizationProvider;
        }

        public async Task<long> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var rolDto = new RoleDto
            {
                TenantId = SessionContext.TenantId,
                Name = request.Name,
                DisplayName = request.DisplayName,
                IsActive = request.IsActive
            };

            var permisoList = request.GrantedPermissionNames ?? new List<string>();
            var pNames = _authorizationProvider.GetPermissionsFromNamesByValidating(permisoList);

            rolDto.Id = await _roleManager.AddRoleAsync(rolDto, pNames.Select(m => m.DisplayName).ToList());

            _authorizationProvider.GetPermissionsFromNamesByValidating(permisoList);
            var permisoDtoList = permisoList.Select(s => new PermissionDto { Name = s }).ToList();
            await _roleManager.ReplacePermissionAsync(rolDto.Id.Value, permisoDtoList);

            return rolDto.Id.Value;
        }
    }
}
