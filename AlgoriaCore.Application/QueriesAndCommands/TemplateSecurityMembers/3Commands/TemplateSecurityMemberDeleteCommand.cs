using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers
{
    public class TemplateSecurityMemberDeleteCommand : IRequest<long>
    {
        public long Template { get; set; }
        public long Id { get; set; }
        public SecurityMemberType Type { get; set; }
        public SecurityMemberLevel Level { get; set; }
    }
}