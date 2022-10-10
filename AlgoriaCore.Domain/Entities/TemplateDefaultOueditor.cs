using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class TemplateDefaultOUEditor : Entity<long>
    {
        public long Template { get; set; }
        public long OrgUnit { get; set; }
        public bool IsExecutor { get; set; }

        public virtual OrgUnit OrgUnitNavigation { get; set; }
        public virtual Template TemplateNavigation { get; set; }
    }
}
