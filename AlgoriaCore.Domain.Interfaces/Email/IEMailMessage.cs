using System.Collections.Generic;

namespace AlgoriaCore.Domain.Interfaces.Email
{
    public interface IEmailMessage
    {
        List<IEmailAddress> ToAddresses { get; set; }
        List<IEmailAddress> CcAddresses { get; set; }
        List<IEmailAddress> BccAddresses { get; set; }
        List<IEmailAddress> FromAddresses { get; set; }
        List<IEmailAttachment> Attachments { get; set; }
        string Subject { get; set; }
        string Content { get; set; }
    }
}
