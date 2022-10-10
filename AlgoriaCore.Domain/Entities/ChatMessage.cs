using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;

namespace AlgoriaCore.Domain.Entities
{
    public partial class ChatMessage : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long UserId { get; set; }
        public int? FriendTenantId { get; set; }
        public long FriendUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Message { get; set; }
        public byte? State { get; set; }
        public byte? Side { get; set; }

        public virtual Tenant FriendTenant { get; set; }
        public virtual User FriendUser { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual User User { get; set; }
    }
}
