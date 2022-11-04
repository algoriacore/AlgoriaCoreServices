using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;

namespace AlgoriaCore.Domain.Entities
{
    public partial class help : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public int LanguageId { get; set; }
        public string HelpKey { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }

        public virtual Language Language { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual helptxt helptxt { get; set; }
    }
}
