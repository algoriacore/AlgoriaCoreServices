using AlgoriaCore.Domain.Entities.Base;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    #nullable enable
    public partial class mailgrouptxt : Entity<long>
    {
        public long? mailgroup { get; set; }
        public byte? type { get; set; }
        public string? body { get; set; }

        public virtual mailgroup? mailgroupNavigation { get; set; }
    }
}
