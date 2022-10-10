using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class OrgUnitUser : Entity<long>
    {
        public long OrgUnit { get; set; }
        public long User { get; set; }

        public virtual OrgUnit OrgUnitNavigation { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
