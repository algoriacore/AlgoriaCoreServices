using AlgoriaCore.Application.Managers.Templates.Dto;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers
{
    public class TemplateSecurityMemberResponse
    {
        public long Id { get; set; }
        public long Template { get; set; }
        public SecurityMemberType Type { get; set; }
        public long Member { get; set; }
        public SecurityMemberLevel Level { get; set; }
        public bool IsExecutor { get; set; }
    }
}