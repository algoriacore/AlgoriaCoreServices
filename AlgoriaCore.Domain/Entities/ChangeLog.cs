using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class ChangeLog : Entity<long>, IMayHaveTenant
    {
        public ChangeLog()
        {
            ChangeLogDetail = new HashSet<ChangeLogDetail>();
        }

        public long? UserId { get; set; }
        public int? TenantId { get; set; }
        public string? table { get; set; }
        public string? key { get; set; }
        public DateTime? datetime { get; set; }

        public virtual Tenant? Tenant { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<ChangeLogDetail> ChangeLogDetail { get; set; }
    }
}
