using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;

namespace AlgoriaCore.Domain.Entities
{
    public partial class Friendship : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long UserId { get; set; }
        public int? FriendTenantId { get; set; }
        public long FriendUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public string FriendNickname { get; set; }
        public byte? State { get; set; }

        public virtual Tenant FriendTenant { get; set; }
        public virtual User FriendUser { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual User User { get; set; }
    }
}
