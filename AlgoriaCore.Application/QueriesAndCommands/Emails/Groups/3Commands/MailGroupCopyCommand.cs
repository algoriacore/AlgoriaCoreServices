using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands
{
    public class MailGroupCopyCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
    }
}
