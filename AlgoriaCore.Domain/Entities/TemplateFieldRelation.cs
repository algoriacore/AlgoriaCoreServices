using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;

namespace AlgoriaCore.Domain.Entities
{
    public partial class TemplateFieldRelation : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long TemplateField { get; set; }
        public long TemplateFieldRelation1 { get; set; }

        public virtual TemplateField TemplateFieldNavigation { get; set; }
        public virtual TemplateField TemplateFieldRelation1Navigation { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
