using System;

namespace AlgoriaCore.Application.Managers.ChatRooms.Dto
{
    public class ChatRoomDto
    {
        public long? Id { get; set; }
        public int? TenantId { get; set; }
        public long? UserCreator { get; set; }
        public string UserCreatorDesc { get; set; }
        public string ChatRoomId { get; set; }
        public string Name { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Description { get; set; }
    }
}
