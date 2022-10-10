using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetListQuery : PageListByDto, IRequest<PagedResultDto<OrgUnitForListResponse>>
    {
        public long? ParentOU { get; set; }
        public byte? Level { get; set; }
    }
}
