using AlgoriaCore.Domain.Entities.Base;
using System;

namespace AlgoriaCore.Domain.Entities
{
    public partial class MailServiceMailStatus : Entity<long>
    {
        public long MailServiceMail { get; set; }
        public DateTime? SentTime { get; set; }
        public byte Status { get; set; }
        public string Error { get; set; }

        public virtual MailServiceMail MailServiceMailNavigation { get; set; }
    }
}
