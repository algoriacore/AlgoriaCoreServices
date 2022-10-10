using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;

namespace AlgoriaCore.Domain.Entities
{
    public partial class OUUsersSecurity : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long? EfectiveId { get; set; }
        public string Name { get; set; } = null!;
        public byte Level { get; set; }
        public long User { get; set; }
    }
}
