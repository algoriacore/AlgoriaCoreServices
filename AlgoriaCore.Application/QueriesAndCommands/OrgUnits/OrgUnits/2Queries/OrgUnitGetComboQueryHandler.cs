using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using AlgoriaCore.Application.Managers.OrgUnits.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetComboQueryHandler : BaseCoreClass, IRequestHandler<OrgUnitGetComboQuery, List<ComboboxItemDto>>
    {
        private readonly OrgUnitManager _manager;

        public OrgUnitGetComboQueryHandler(ICoreServices coreServices, OrgUnitManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ComboboxItemDto>> Handle(OrgUnitGetComboQuery request, CancellationToken cancellationToken)
        {
            return await _manager.GetOrgUnitComboAsync(new OrgUnitComboFilterDto() { Filter = request.Filter });
        }
    }
}
