using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class TemplateToDoStatus : Entity<long>, IMayHaveTenant, ISoftDelete
    {
        public TemplateToDoStatus()
        {
            ToDoActivity = new HashSet<ToDoActivity>();
        }

        public long Template { get; set; }
        public int? TenantId { get; set; }
        public byte? Type { get; set; }
        public string Name { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Template TemplateNavigation { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<ToDoActivity> ToDoActivity { get; set; }
    }
}
