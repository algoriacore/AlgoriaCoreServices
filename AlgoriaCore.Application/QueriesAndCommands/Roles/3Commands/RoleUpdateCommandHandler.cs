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
    public class RoleUpdateCommandHandler : BaseCoreClass, IRequestHandler<RoleUpdateCommand, long>
    {
        private readonly RoleManager _roleManager;
        private readonly IAppAuthorizationProvider _authorizationProvider;

        public RoleUpdateCommandHandler(ICoreServices coreServices, RoleManager roleManager, IAppAuthorizationProvider authorizationProvider) : base(coreServices)
        {
            _roleManager = roleManager;
            _authorizationProvider = authorizationProvider;
        }

        public async Task<long> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var rolDto = new RoleDto
            {
                Id = request.Id,
                TenantId = SessionContext.TenantId,
                Name = request.Name,
                DisplayName = request.DisplayName,
                IsActive = request.IsActive,
                IsDeleted = request.IsDeleted
            };

            var permisoList = request.GrantedPermissionNames ?? new List<string>();
            var pNames = _authorizationProvider.GetPermissionsFromNamesByValidating(permisoList);

            await _roleManager.UpdateRoleAsync(rolDto, pNames.Select(m => m.DisplayName).ToList());

            var permisoDtoList = permisoList.Select(s => new PermissionDto
            {
                Name = s,
                DisplayName = pNames.Any(a => a.Name == s) ? pNames.FirstOrDefault(a => a.Name == s)?.DisplayName : null
            }).ToList();

            await _roleManager.ReplacePermissionAsync(rolDto.Id.Value, permisoDtoList);

            return request.Id;
        }
    }
}
