using AlgoriaCore.Domain.Entities.Base;
using System;

namespace AlgoriaCore.Domain.Entities
{
    public partial class ChatRoomChatFile : Entity<long>
    {
        public long ChatRoomChat { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public Guid? File { get; set; }

        public virtual ChatRoomChat ChatRoomChatNavigation { get; set; }
    }
}
