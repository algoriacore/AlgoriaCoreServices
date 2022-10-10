using System;

namespace AlgoriaCore.Application.Managers.ChatRooms.Dto
{
    public class ChatRoomChatUserTaggedDto
    {
        public long? Id { get; set; }
        public long ChatRoomChat { get; set; }
        public long UserTagged { get; set; }
        public string UserTaggedDesc { get; set; }

        public string UserTaggedEmail { get; set; }
    }
}
