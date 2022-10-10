using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;

namespace AlgoriaCore.Domain.Entities
{
    public partial class help : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public int LanguageId { get; set; }
        public string HelpKey { get; set; } = null!;
        public string? DisplayName { get; set; }
        public bool? IsActive { get; set; }

        public virtual Language Language { get; set; } = null!;
        public virtual Tenant? Tenant { get; set; }
        public virtual helptxt helptxt { get; set; } = null!;
    }
}
