using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnitUsers.OrgUnitUsers
{
    public class OrgUnitUserGetListQuery : PageListByDto, IRequest<PagedResultDto<OrgUnitUserForListResponse>>
    {
        public long? OrgUnit { get; set; }
    }
}
