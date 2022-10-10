using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;

namespace AlgoriaCore.Domain.Entities
{
    public partial class mailtemplate : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long? mailgroup { get; set; }
        public string? mailkey { get; set; }
        public string? DisplayName { get; set; }
        public string? SendTo { get; set; }
        public string? CopyTo { get; set; }
        public string? BlindCopyTo { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public bool? IsActive { get; set; }

        public virtual Tenant? Tenant { get; set; }
        public virtual mailgroup? mailgroupNavigation { get; set; }
    }
}
