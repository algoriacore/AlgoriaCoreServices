using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using AlgoriaCore.Application.Managers.OrgUnits.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitCreateCommandHandler : BaseCoreClass, IRequestHandler<OrgUnitCreateCommand, long>
    {
        private readonly OrgUnitManager _manager;

        public OrgUnitCreateCommandHandler(ICoreServices coreServices, OrgUnitManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(OrgUnitCreateCommand request, CancellationToken cancellationToken)
        {
            OrgUnitDto dto = new OrgUnitDto()
            {
                ParentOU = request.ParentOU,
                Name = request.Name
            };

            return await _manager.CreateOrgUnitAsync(dto);
        }
    }
}
