using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers
{
    public class TemplateSecurityMemberCreateCommand : IRequest<long>
    {
        public long Template { get; set; }
        public SecurityMemberType Type { get; set; }
        public long Member { get; set; }
        public SecurityMemberLevel Level { get; set; }
        public bool IsExecutor { get; set; }
    }
}