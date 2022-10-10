using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class TemplateDefaultUserEditor : Entity<long>
    {
        public long Template { get; set; }
        public long User { get; set; }
        public bool IsExecutor { get; set; }

        public virtual Template TemplateNavigation { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
