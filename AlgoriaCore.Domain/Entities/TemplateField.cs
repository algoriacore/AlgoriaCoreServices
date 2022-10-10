using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class TemplateField : Entity<long>, IMayHaveTenant, ISoftDelete
    {
        public TemplateField()
        {
            TemplateFieldOption = new HashSet<TemplateFieldOption>();
            TemplateFieldRelationTemplateFieldRelation1Navigation = new HashSet<TemplateFieldRelation>();
        }

        public int? TenantId { get; set; }
        public long? TemplateSection { get; set; }
        public string Name { get; set; }
        public string FieldName { get; set; }
        public short? FieldType { get; set; }
        public short? FieldSize { get; set; }
        public short? FieldControl { get; set; }
        public string InputMask { get; set; }
        public bool? HasKeyFilter { get; set; }
        public string KeyFilter { get; set; }
        public byte? Status { get; set; }
        public bool? IsRequired { get; set; }
        public bool? ShowOnGrid { get; set; }
        public short? Order { get; set; }
        public bool? InheritSecurity { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual TemplateSection TemplateSectionNavigation { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual TemplateFieldRelation TemplateFieldRelationTemplateFieldNavigation { get; set; }
        public virtual ICollection<TemplateFieldOption> TemplateFieldOption { get; set; }
        public virtual ICollection<TemplateFieldRelation> TemplateFieldRelationTemplateFieldRelation1Navigation { get; set; }
    }
}
