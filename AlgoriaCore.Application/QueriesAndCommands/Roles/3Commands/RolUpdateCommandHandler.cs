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
    public class RolUpdateCommandHandler : BaseCoreClass, IRequestHandler<RolUpdateCommand, long>
    {
        private readonly RolManager _rolManager;
        private readonly IAppAuthorizationProvider _authorizationProvider;

        public RolUpdateCommandHandler(ICoreServices coreServices, RolManager rolManager, IAppAuthorizationProvider authorizationProvider) : base(coreServices)
        {
            _rolManager = rolManager;
            _authorizationProvider = authorizationProvider;
        }

        public async Task<long> Handle(RolUpdateCommand request, CancellationToken cancellationToken)
        {
            var rolDto = new RolDto
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

            await _rolManager.UpdateRolAsync(rolDto, pNames.Select(m => m.DisplayName).ToList());

            var permisoDtoList = permisoList.Select(s => new PermissionDto
            {
                Name = s,
                DisplayName = pNames.Any(a => a.Name == s) ? pNames.FirstOrDefault(a => a.Name == s).DisplayName : null
            }).ToList();

            await _rolManager.ReplacePermissionAsync(rolDto.Id.Value, permisoDtoList);

            return request.Id;
        }
    }
}
