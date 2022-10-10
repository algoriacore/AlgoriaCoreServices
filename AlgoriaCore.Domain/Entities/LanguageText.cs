
using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class LanguageText : Entity<long>
    {
        public int? LanguageId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public virtual Language Language { get; set; }
    }
}
