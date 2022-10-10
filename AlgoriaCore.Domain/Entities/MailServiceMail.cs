using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;

namespace AlgoriaCore.Domain.Entities
{
    public partial class MailServiceMail : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long MailServiceRequest { get; set; }
        public bool IsLocalConfig { get; set; }
        public string Sendto { get; set; }
        public string CopyTo { get; set; }
        public string BlindCopyTo { get; set; }
        public string Subject { get; set; }

        public virtual MailServiceRequest MailServiceRequestNavigation { get; set; }
        public virtual MailServiceMailBody MailServiceMailBody { get; set; }
        public virtual MailServiceMailConfig MailServiceMailConfig { get; set; }
        public virtual MailServiceMailStatus MailServiceMailStatus { get; set; }
    }
}
