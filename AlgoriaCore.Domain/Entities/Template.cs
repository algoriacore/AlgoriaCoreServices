using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
#nullable enable
    public partial class Template : Entity<long>, IMayHaveTenant, ISoftDelete
    {
        public Template()
        {
            TemplateDefaultOUEditor = new HashSet<TemplateDefaultOUEditor>();
            TemplateDefaultOUReader = new HashSet<TemplateDefaultOUReader>();
            TemplateDefaultUserEditor = new HashSet<TemplateDefaultUserEditor>();
            TemplateDefaultUserReader = new HashSet<TemplateDefaultUserReader>();
            TemplateQuery = new HashSet<TemplateQuery>();
            TemplateSection = new HashSet<TemplateSection>();
            TemplateToDoStatus = new HashSet<TemplateToDoStatus>();
        }

        public int? TenantId { get; set; }
        public string? RGBColor { get; set; }
        public string NameSingular { get; set; } = null!;
        public string NamePlural { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? Icon { get; set; }
        public string? TableName { get; set; }
        public bool? IsTableGenerated { get; set; }
        public bool? HasChatRoom { get; set; }
        public bool? IsActivity { get; set; }
        public bool? HasSecurity { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Tenant? Tenant { get; set; }
        public virtual ICollection<TemplateDefaultOUEditor> TemplateDefaultOUEditor { get; set; }
        public virtual ICollection<TemplateDefaultOUReader> TemplateDefaultOUReader { get; set; }
        public virtual ICollection<TemplateDefaultUserEditor> TemplateDefaultUserEditor { get; set; }
        public virtual ICollection<TemplateDefaultUserReader> TemplateDefaultUserReader { get; set; }
        public virtual ICollection<TemplateQuery> TemplateQuery { get; set; }
        public virtual ICollection<TemplateSection> TemplateSection { get; set; }
        public virtual ICollection<TemplateToDoStatus> TemplateToDoStatus { get; set; }
    }
}
