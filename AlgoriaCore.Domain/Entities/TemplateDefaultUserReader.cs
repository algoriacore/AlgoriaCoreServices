using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class TemplateDefaultUserReader : Entity<long>
    {
        public long Template { get; set; }
        public long User { get; set; }

        public virtual Template TemplateNavigation { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
