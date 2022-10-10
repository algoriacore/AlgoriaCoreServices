using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Permissions;
using AlgoriaCore.Application.Managers.Permissions.Dto;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Extensions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Authorization.Permissions
{
    public class PermissionGetIsGrantedQueryHandler : BaseCoreClass, IRequestHandler<PermissionGetIsGrantedQuery, bool>
    {
        private readonly PermissionManager _managerPermission;

        public PermissionGetIsGrantedQueryHandler(ICoreServices coreServices
        , PermissionManager managerPermission): base(coreServices)
        {
            _managerPermission = managerPermission;
        }

        public async Task<bool> Handle(PermissionGetIsGrantedQuery request, CancellationToken cancellationToken)
        {
            PermissionGetIsGrantedDto dto = new PermissionGetIsGrantedDto()
            {
                RequiresAll = request.RequiresAll,
                PermissionNames = request.PermissionNames
            };

            if (request.IsTemplateProcess && request.Template.HasValue)
            {
                dto.PermissionNames = dto.PermissionNames.Select(
                    p => AppPermissions.IsPermissionNameForProcess(p)
                    ? AppPermissions.CalculatePermissionNameForProcess(p, _managerPermission.SessionContext.TenantId, request.Template.Value)
                    : p
                ).ToArray();
            }

            if (request.IsCatalogCustomImpl && !request.Catalog.IsNullOrWhiteSpace())
            {
                dto.PermissionNames = dto.PermissionNames.Select(
                    p => AppPermissions.IsPermissionNameForCatalogCustom(p)
                    ? AppPermissions.CalculatePermissionNameForCatalogCustom(p, _managerPermission.SessionContext.TenantId, request.Catalog)
                    : p
                ).ToArray();
            }

            return await _managerPermission.IsGrantedAsync(dto);
        }
    }
}
