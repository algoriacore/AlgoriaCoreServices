using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SecurityMembers
{
    public class ProcessSecurityMemberGetListQuery : PageListByDto, IRequest<PagedResultDto<ProcessSecurityMemberForListResponse>>
    {
        public long Template { get; set; }
        public long Parent { get; set; }
        public SecurityMemberType? Type { get; set; }
        public SecurityMemberLevel? Level { get; set; }
    }
}
