using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Roles;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands
{
    public class RoleDeleteCommandHandler : BaseCoreClass, IRequestHandler<RoleDeleteCommand, long>
    {
        private readonly RoleManager _roleManager;

        public RoleDeleteCommandHandler(ICoreServices coreServices, RoleManager roleManager) : base(coreServices)
        {
            _roleManager = roleManager;
        }

        public async Task<long> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
        {
            return await _roleManager.DeleteRoleAsync(request.Id);
        }
    }
}
