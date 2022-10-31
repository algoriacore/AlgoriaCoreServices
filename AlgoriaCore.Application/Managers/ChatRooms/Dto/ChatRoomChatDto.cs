using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.ChatRooms.Dto
{
    public class ChatRoomChatDto
    {
        public long? Id { get; set; }
        public int? TenantId { get; set; }
        public long ChatRoom { get; set; }
        public string ChatRoomId { get; set; }
        public long? User { get; set; }
        public string UserDesc { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Comment { get; set; }
        public bool HasFiles { get; set; }

        public List<ChatRoomChatUserTaggedDto> TaggedUsers { get; set; }
        public List<ChatRoomChatFileDto> Files { get; set; }
		public string UserLogin { get; set; }

		public ChatRoomChatDto() {
            TaggedUsers = new List<ChatRoomChatUserTaggedDto>();
            Files = new List<ChatRoomChatFileDto>();
        }
    }
}
