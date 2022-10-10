using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries
{
    public class RolGetByIdQueryHandler : BaseCoreClass, IRequestHandler<RolGetByIdQuery, RolResponse>
    {
        private readonly RolManager _rolManager;
   
        public RolGetByIdQueryHandler(RolManager rolManager, ICoreServices coreServices) : base(coreServices)
        {
            _rolManager = rolManager;
        }

        public async Task<RolResponse> Handle(RolGetByIdQuery request, CancellationToken cancellationToken)
        {
            RolResponse rolResponse = new RolResponse();

            var rolDto = await _rolManager.GetRoleByIdAsync(request.Id);

            rolResponse.Id = rolDto.Id.Value;
            rolResponse.TenantId = SessionContext.TenantId;
            rolResponse.Name = rolDto.Name;
            rolResponse.DisplayName = rolDto.DisplayName;
            rolResponse.IsActive = rolDto.IsActive;

            return rolResponse;
        }
    }
}
