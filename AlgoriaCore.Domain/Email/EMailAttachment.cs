using AlgoriaCore.Domain.Interfaces.Email;

namespace AlgoriaCore.Domain.Email
{
    public class EmailAttachment : IEmailAttachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileArray { get; set; }
    }
}
