using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitDeleteCommandHandler : BaseCoreClass, IRequestHandler<OrgUnitDeleteCommand, long>
    {
        private readonly OrgUnitManager _manager;

        public OrgUnitDeleteCommandHandler(ICoreServices coreServices, OrgUnitManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(OrgUnitDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.DeleteOrgUnitAsync(request.Id);

            return request.Id;
        }
    }
}
