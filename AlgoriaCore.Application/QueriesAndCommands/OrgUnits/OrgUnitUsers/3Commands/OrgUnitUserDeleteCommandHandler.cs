using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnitUsers.OrgUnitUsers
{
    public class OrgUnitUserDeleteCommandHandler : BaseCoreClass, IRequestHandler<OrgUnitUserDeleteCommand, long>
    {
        private readonly OrgUnitManager _manager;

        public OrgUnitUserDeleteCommandHandler(ICoreServices coreServices, OrgUnitManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(OrgUnitUserDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.DeleteOrgUnitUserAsync(request.Id);

            return request.Id;
        }
    }
}
