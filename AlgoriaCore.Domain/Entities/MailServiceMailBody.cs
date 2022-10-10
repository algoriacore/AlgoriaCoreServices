using AlgoriaCore.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class MailServiceMailBody : Entity<long>
    {
        public MailServiceMailBody()
        {
            MailServiceMailAttach = new HashSet<MailServiceMailAttach>();
        }

        public long MailServiceMail { get; set; }
        public string Body { get; set; }

        public virtual MailServiceMail MailServiceMailNavigation { get; set; }
        public virtual ICollection<MailServiceMailAttach> MailServiceMailAttach { get; set; }
    }
}
