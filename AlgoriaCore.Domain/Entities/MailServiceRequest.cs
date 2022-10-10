using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class MailServiceRequest : Entity<long>, IMustHaveTenant
    {
        public MailServiceRequest()
        {
            MailServiceMail = new HashSet<MailServiceMail>();
        }

        public int TenantId { get; set; }
        public long UserCreator { get; set; }
        public DateTime? CreationTime { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual User UserCreatorNavigation { get; set; }
        public virtual ICollection<MailServiceMail> MailServiceMail { get; set; }
    }
}
