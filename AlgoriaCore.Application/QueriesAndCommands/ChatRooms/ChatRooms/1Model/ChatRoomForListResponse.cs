using System;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomForListResponse
    {
        public long Id { get; set; }
        public string ChatRoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserCreatorDesc { get; set; }
        public DateTime? CreationTime { get; set; }
    }
}