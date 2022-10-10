using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._3Commands
{
    public class MailTemplateSendTestCommand : IRequest<int>
    {
        public long MailGroup { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
