using System;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomResponse
    {
        public long Id { get; set; }
        public long? UserCreator { get; set; }
        public string UserCreatorDesc { get; set; }
        public string ChatRoomId { get; set; }
        public string Name { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Description { get; set; }
    }
}