using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class TemplateFieldOption : Entity<long>
    {
        public long? TemplateField { get; set; }
        public int value { get; set; }
        public string Description { get; set; } = null!;

        public virtual TemplateField TemplateFieldNavigation { get; set; }
    }
}
