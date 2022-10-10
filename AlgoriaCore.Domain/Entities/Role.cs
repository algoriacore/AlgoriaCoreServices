
using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class Role : Entity<long>, IMayHaveTenant, ISoftDelete
    {
        public Role()
        {
            Permission = new HashSet<Permission>();
            UserRole = new HashSet<UserRole>();
        }
		
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<Permission> Permission { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
