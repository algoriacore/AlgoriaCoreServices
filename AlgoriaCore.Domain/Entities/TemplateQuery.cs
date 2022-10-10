using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class TemplateQuery : Entity<long>
    {
        public long? Template { get; set; }
        public byte? QueryType { get; set; }
        public string Query { get; set; }

        public virtual Template TemplateNavigation { get; set; }
    }
}
