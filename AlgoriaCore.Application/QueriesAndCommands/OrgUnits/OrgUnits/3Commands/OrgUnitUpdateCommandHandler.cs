using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using AlgoriaCore.Application.Managers.OrgUnits.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitUpdateCommandHandler : BaseCoreClass, IRequestHandler<OrgUnitUpdateCommand, long>
    {
        private readonly OrgUnitManager _managerOrgUnit;

        public OrgUnitUpdateCommandHandler(ICoreServices coreServices
        , OrgUnitManager managerOrgUnit): base(coreServices)
        {
            _managerOrgUnit = managerOrgUnit;
        }

        public async Task<long> Handle(OrgUnitUpdateCommand request, CancellationToken cancellationToken)
        {
            OrgUnitDto dto = new OrgUnitDto()
            {
                Id = request.Id,
                Name = request.Name
            };

            await _managerOrgUnit.UpdateOrgUnitAsync(dto);

            return dto.Id.Value;
        }
    }
}
