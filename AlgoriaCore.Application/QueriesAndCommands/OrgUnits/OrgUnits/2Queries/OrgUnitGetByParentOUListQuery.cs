using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetByParentOUListQuery : PageListByDto, IRequest<List<OrgUnitForListResponse>>
    {
        public long ParentOU { get; set; }
    }
}
