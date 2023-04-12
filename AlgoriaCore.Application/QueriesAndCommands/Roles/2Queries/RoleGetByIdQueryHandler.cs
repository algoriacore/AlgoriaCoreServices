using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries
{
    public class RoleGetByIdQueryHandler : BaseCoreClass, IRequestHandler<RoleGetByIdQuery, RoleResponse>
    {
        private readonly RoleManager _roleManager;
   
        public RoleGetByIdQueryHandler(RoleManager rolManager, ICoreServices coreServices) : base(coreServices)
        {
            _roleManager = rolManager;
        }

        public async Task<RoleResponse> Handle(RoleGetByIdQuery request, CancellationToken cancellationToken)
        {
            RoleResponse rolResponse = new RoleResponse();

            var rolDto = await _roleManager.GetRoleByIdAsync(request.Id);

            rolResponse.Id = rolDto.Id.Value;
            rolResponse.TenantId = SessionContext.TenantId;
            rolResponse.Name = rolDto.Name;
            rolResponse.DisplayName = rolDto.DisplayName;
            rolResponse.IsActive = rolDto.IsActive;

            return rolResponse;
        }
    }
}
