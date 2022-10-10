using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Roles;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands
{
    public class RolDeleteCommandHandler : BaseCoreClass, IRequestHandler<RolDeleteCommand, long>
    {
        private readonly RolManager _rolManager;

        public RolDeleteCommandHandler(ICoreServices coreServices, RolManager rolManager) : base(coreServices)
        {
            _rolManager = rolManager;
        }

        public async Task<long> Handle(RolDeleteCommand request, CancellationToken cancellationToken)
        {
            return await _rolManager.DeleteRolAsync(request.Id);
        }
    }
}
