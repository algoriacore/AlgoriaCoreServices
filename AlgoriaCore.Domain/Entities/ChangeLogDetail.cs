
using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class ChangeLogDetail : Entity<long>
    {
        public long? changelog { get; set; }
        public string? data { get; set; }

        public virtual ChangeLog? changelogNavigation { get; set; }
    }
}
