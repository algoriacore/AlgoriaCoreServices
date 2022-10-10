using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class ChatRoom : Entity<long>, IMayHaveTenant
    {
        public ChatRoom()
        {
            ChatRoomChat = new HashSet<ChatRoomChat>();
        }

        public int? TenantId { get; set; }
        public long? UserCreator { get; set; }
        public string ChatRoomId { get; set; }
        public string Name { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Description { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual User UserCreatorNavigation { get; set; }
        public virtual ICollection<ChatRoomChat> ChatRoomChat { get; set; }
    }
}
