using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers
{
    public class TemplateSecurityMemberGetListQuery : PageListByDto, IRequest<PagedResultDto<TemplateSecurityMemberForListResponse>>
    {
        public long Template { get; set; }
        public SecurityMemberType? Type { get; set; }
        public SecurityMemberLevel? Level { get; set; }
    }
}
