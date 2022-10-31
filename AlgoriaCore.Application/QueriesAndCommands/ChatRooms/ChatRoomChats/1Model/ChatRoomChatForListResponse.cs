using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats
{
    public class ChatRoomChatForListResponse
    {
        public long Id { get; set; }
        public long ChatRoom { get; set; }
        public long? User { get; set; }
        public string UserDesc { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Comment { get; set; }

        public List<ChatRoomChatFileResponse> Files { get; set; }
		public string UserLogin { get; set; }

		public ChatRoomChatForListResponse()
        {
            Files = new List<ChatRoomChatFileResponse>();
        }
    }
}