using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class MailServiceMailConfig : Entity<long>
    {
        public long MailServiceMail { get; set; }
        public string? Sender { get; set; }
        public string? SenderDisplay { get; set; }
        public string? SMPTHost { get; set; }
        public short? SMPTPort { get; set; }
        public bool? IsSSL { get; set; }
        public bool? UseDefaultCredential { get; set; }
        public string? Domain { get; set; }
        public string? MailUser { get; set; }
        public string? MailPassword { get; set; }

        public virtual MailServiceMail MailServiceMailNavigation { get; set; } = null!;
    }
}
