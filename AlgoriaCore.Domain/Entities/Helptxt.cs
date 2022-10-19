using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    #nullable enable
	public partial class helptxt : Entity<long>
    {
        public long help { get; set; }
        public string? body { get; set; }

        public virtual help helpNavigation { get; set; } = null!;
    }
}
