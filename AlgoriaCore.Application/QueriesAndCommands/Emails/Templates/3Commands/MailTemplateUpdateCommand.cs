using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._3Commands
{
    public class MailTemplateUpdateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public long? MailGroup { get; set; }
        public string MailKey { get; set; }
        public string DisplayName { get; set; }
        public string SendTo { get; set; }
        public string CopyTo { get; set; }
        public string BlindCopyTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool? IsActive { get; set; }
    }
}
