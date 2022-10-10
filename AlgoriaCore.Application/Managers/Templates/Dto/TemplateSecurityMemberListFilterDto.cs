using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.Templates.Dto
{
    public class TemplateSecurityMemberListFilterDto : PageListByDto
    {
        public long Template { get; set; }
        public SecurityMemberType? Type { get; set; }
        public SecurityMemberLevel? Level { get; set; }
    }
}
