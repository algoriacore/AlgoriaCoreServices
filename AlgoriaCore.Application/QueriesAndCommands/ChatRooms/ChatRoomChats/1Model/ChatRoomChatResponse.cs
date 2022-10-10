using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats
{
    public class ChatRoomChatResponse
    {
        public long Id { get; set; }
        public long ChatRoom { get; set; }
        public long? User { get; set; }
        public string UserDesc { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Comment { get; set; }

        public List<ChatRoomChatFileResponse> Files { get; set; }

        public ChatRoomChatResponse() {
            Files = new List<ChatRoomChatFileResponse>();
        }
    }

    public class ChatRoomChatFileResponse
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public Guid? File { get; set; }
    }
}