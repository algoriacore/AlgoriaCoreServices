using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.OrgUnits;
using AlgoriaCore.Application.Managers.OrgUnits.Dto;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetByParentOUListQueryHandler : BaseCoreClass, IRequestHandler<OrgUnitGetByParentOUListQuery, List<OrgUnitForListResponse>>
    {
        private readonly OrgUnitManager _manager;

        public OrgUnitGetByParentOUListQueryHandler(ICoreServices coreServices, OrgUnitManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<OrgUnitForListResponse>> Handle(OrgUnitGetByParentOUListQuery request, CancellationToken cancellationToken)
        {
            List<OrgUnitDto> list = await _manager.GetOrgUnitByParentListAsync(request.ParentOU);

            return list.Select(p => new OrgUnitForListResponse()
            {
                Id = p.Id.Value,
                ParentOU = p.ParentOU,
                ParentOUDesc = p.ParentOUDesc,
                Name = p.Name,
                Level = p.Level,
                HasChildren = p.HasChildren,
                Size = p.Size
            }).ToList();
        }
    }
}
