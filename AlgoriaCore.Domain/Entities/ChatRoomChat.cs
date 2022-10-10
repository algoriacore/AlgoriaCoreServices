using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Entities
{
    public partial class ChatRoomChat : Entity<long>, IMayHaveTenant
    {
        public ChatRoomChat()
        {
            ChatRoomChatFile = new HashSet<ChatRoomChatFile>();
            ChatRoomChatUserTagged = new HashSet<ChatRoomChatUserTagged>();
        }

        public int? TenantId { get; set; }
        public long ChatRoom { get; set; }
        public long? User { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Comment { get; set; }

        public virtual ChatRoom ChatRoomNavigation { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual User UserNavigation { get; set; }
        public virtual ICollection<ChatRoomChatFile> ChatRoomChatFile { get; set; }
        public virtual ICollection<ChatRoomChatUserTagged> ChatRoomChatUserTagged { get; set; }
    }
}
