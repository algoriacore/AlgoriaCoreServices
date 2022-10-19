using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;

namespace AlgoriaCore.Domain.Entities
{
#nullable enable
    public partial class Setting : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long? UserId { get; set; }
        public string? Name { get; set; }
        public string? value { get; set; }

        public virtual Tenant? Tenant { get; set; }
        public virtual User? User { get; set; }
    }
}
