using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class OrgUnit : Entity<long>, IMayHaveTenant, ISoftDelete
    {
        public OrgUnit()
        {
            InverseParentOUNavigation = new HashSet<OrgUnit>();
            OrgUnitUser = new HashSet<OrgUnitUser>();
        }

        public int? TenantId { get; set; }
        public long? ParentOU { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
        public bool IsDeleted { get; set; }

        public virtual OrgUnit ParentOUNavigation { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<OrgUnit> InverseParentOUNavigation { get; set; }
        public virtual ICollection<OrgUnitUser> OrgUnitUser { get; set; }
    }
}
