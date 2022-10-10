using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using AlgoriaCore.Application.Managers.OrgUnits.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnitUsers.OrgUnitUsers
{
    public class OrgUnitUserCreateCommandHandler : BaseCoreClass, IRequestHandler<OrgUnitUserCreateCommand, long>
    {
        private readonly OrgUnitManager _manager;

        public OrgUnitUserCreateCommandHandler(ICoreServices coreServices, OrgUnitManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(OrgUnitUserCreateCommand request, CancellationToken cancellationToken)
        {
            OrgUnitUserDto dto = new OrgUnitUserDto()
            {
                OrgUnit = request.OrgUnit,
                User = request.User
            };

            return await _manager.CreateOrgUnitUserAsync(dto);
        }
    }
}
