using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands
{
    public class MailGroupCreateCommand : IRequest<long>
    {
        public string DisplayName { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
    }
}
