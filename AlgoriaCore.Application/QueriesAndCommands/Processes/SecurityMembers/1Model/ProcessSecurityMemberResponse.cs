using AlgoriaCore.Application.Managers.Templates.Dto;

namespace AlgoriaCore.Application.QueriesAndCommands.SecurityMembers
{
    public class ProcessSecurityMemberResponse
    {
        public long Id { get; set; }
        public long Parent { get; set; }
        public SecurityMemberType Type { get; set; }
        public long Member { get; set; }
        public SecurityMemberLevel Level { get; set; }
        public bool IsExecutor { get; set; }
    }
}