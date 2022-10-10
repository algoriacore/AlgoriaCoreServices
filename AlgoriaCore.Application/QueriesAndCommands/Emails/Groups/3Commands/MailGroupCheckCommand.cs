using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands
{
    public class MailGroupCheckCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}
