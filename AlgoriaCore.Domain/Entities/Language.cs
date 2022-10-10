using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class Language : Entity<int>, IMayHaveTenant
    {
        public Language()
        {
            LanguageText = new HashSet<LanguageText>();
            help = new HashSet<help>();
        }

        public int? TenantId { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public bool? IsActive { get; set; }

        public virtual Tenant? Tenant { get; set; }
        public virtual ICollection<LanguageText> LanguageText { get; set; }
        public virtual ICollection<help> help { get; set; }
    }
}
