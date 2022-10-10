using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Templates.Dto;

namespace AlgoriaCore.Application.Managers.Processes.Dto
{
    public class ProcessSecurityMemberListFilterDto : PageListByDto
    {
        public long Parent { get; set; }
        public long Template { get; set; }
        public SecurityMemberType? Type { get; set; }
        public SecurityMemberLevel? Level { get; set; }
    }
}
