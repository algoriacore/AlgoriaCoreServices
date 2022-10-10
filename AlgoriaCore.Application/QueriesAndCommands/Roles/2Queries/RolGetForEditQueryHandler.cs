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
    public class RolGetForEditQueryHandler : BaseCoreClass, IRequestHandler<RolGetForEditQuery, RolForEditReponse>
    {
        private readonly RolManager _rolManager;

        public RolGetForEditQueryHandler(ICoreServices coreServices, RolManager rolManager) : base(coreServices)
        {
            _rolManager = rolManager;
        }

        public async Task<RolForEditReponse> Handle(RolGetForEditQuery request, CancellationToken cancellationToken)
        {
            var rolDto = await _rolManager.GetRoleByIdAsync(request.Id);

            var response = new RolForEditReponse
            {
                Id = rolDto.Id.Value,
                DisplayName = rolDto.DisplayName,
                Name = rolDto.Name,
                IsActive = rolDto.IsActive
            };

            var ll = await _rolManager.GetAllPermissionsByRoleIdAsync(request.Id);

            response.PermisoList = ll.Select(entidad => new RolPermisoResponse
            {
                Id = entidad.Id,
                Name = entidad.Name
            }).ToList();

            return response;
        }
    }
}
