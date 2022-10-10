using AlgoriaCore.Domain.Interfaces.Email;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Email
{
    public class EmailMessage : IEmailMessage
    {
        public EmailMessage()
        {
            ToAddresses = new List<IEmailAddress>();
            CcAddresses = new List<IEmailAddress>();
            BccAddresses = new List<IEmailAddress>();
            FromAddresses = new List<IEmailAddress>();
            Attachments = new List<IEmailAttachment>();
        }

        public List<IEmailAddress> ToAddresses { get; set; }
        public List<IEmailAddress> CcAddresses { get; set; }
        public List<IEmailAddress> BccAddresses { get; set; }
        public List<IEmailAddress> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<IEmailAttachment> Attachments { get; set; }
    }
}
