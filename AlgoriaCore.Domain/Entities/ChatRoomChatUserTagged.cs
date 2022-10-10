using AlgoriaCore.Domain.Entities.Base;

namespace AlgoriaCore.Domain.Entities
{
    public partial class ChatRoomChatUserTagged : Entity<long>
    {
        public long ChatRoomChat { get; set; }
        public long UserTagged { get; set; }

        public virtual ChatRoomChat ChatRoomChatNavigation { get; set; }
        public virtual User UserTaggedNavigation { get; set; }
    }
}
