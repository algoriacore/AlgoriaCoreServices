using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries
{
    public class RoleGetForEditQueryHandler : BaseCoreClass, IRequestHandler<RoleGetForEditQuery, RoleForEditReponse>
    {
        private readonly RoleManager _roleManager;

        public RoleGetForEditQueryHandler(ICoreServices coreServices, RoleManager rolManager) : base(coreServices)
        {
            _roleManager = rolManager;
        }

        public async Task<RoleForEditReponse> Handle(RoleGetForEditQuery request, CancellationToken cancellationToken)
        {
            var rolDto = await _roleManager.GetRoleByIdAsync(request.Id);

            var response = new RoleForEditReponse
            {
                Id = rolDto.Id.Value,
                DisplayName = rolDto.DisplayName,
                Name = rolDto.Name,
                IsActive = rolDto.IsActive
            };

            var ll = await _roleManager.GetAllPermissionsByRoleIdAsync(request.Id);

            response.PermissionList = ll.Select(entidad => new RolePermisoResponse
            {
                Id = entidad.Id,
                Name = entidad.Name
            }).ToList();

            return response;
        }
    }
}
