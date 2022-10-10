using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;

namespace AlgoriaCore.Domain.Entities
{
    public partial class SampleDateData : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public DateTime? DateTimeData { get; set; }
        public DateTime? DateData { get; set; }
        public TimeSpan? TimeData { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
