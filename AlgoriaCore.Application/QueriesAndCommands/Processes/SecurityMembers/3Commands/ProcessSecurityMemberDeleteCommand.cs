using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SecurityMembers
{
    public class ProcessSecurityMemberDeleteCommand : IRequest<long>
    {
        public long Template { get; set; }
        public long Id { get; set; }
        public SecurityMemberType Type { get; set; }
        public SecurityMemberLevel Level { get; set; }
    }
}