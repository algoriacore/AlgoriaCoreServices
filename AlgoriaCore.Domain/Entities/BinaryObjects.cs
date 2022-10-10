
using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;

namespace AlgoriaCore.Domain.Entities
{
    public partial class BinaryObjects : Entity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public byte[] Bytes { get; set; }
        public string FileName { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
