using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    #nullable enable
    public partial class mailgroup : Entity<long>, IMayHaveTenant
    {
        public mailgroup()
        {
            mailgrouptxt = new HashSet<mailgrouptxt>();
            mailtemplate = new HashSet<mailtemplate>();
        }

        public int? TenantId { get; set; }
        public string? DisplayName { get; set; }

        public virtual Tenant? Tenant { get; set; }
        public virtual ICollection<mailgrouptxt> mailgrouptxt { get; set; }
        public virtual ICollection<mailtemplate> mailtemplate { get; set; }
    }
}
