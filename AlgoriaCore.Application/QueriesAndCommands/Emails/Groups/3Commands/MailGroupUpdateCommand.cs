using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands
{
    public class MailGroupUpdateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
    }
}
