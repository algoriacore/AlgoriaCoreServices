using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
#nullable enable
    public partial class TemplateSection : Entity<long>, IMayHaveTenant, ISoftDelete
    {
        public TemplateSection()
        {
            TemplateField = new HashSet<TemplateField>();
        }

        public int? TenantId { get; set; }
        public long? Template { get; set; }
        public string? Name { get; set; }
        public short? Order { get; set; }
        public string? IconAF { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Template? TemplateNavigation { get; set; }
        public virtual Tenant? Tenant { get; set; }
        public virtual ICollection<TemplateField> TemplateField { get; set; }
    }
}
