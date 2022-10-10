using AlgoriaCore.Application.BaseClases.Dto;

namespace AlgoriaCore.Application.Managers.ChatRooms.Dto
{
    public class ChatRoomChatListFilterDto : PageListByDto
    {
        public long? ChatRoom { get; set; }
        public string ChatRoomId { get; set; }
        public int Skip { get; set; }
        public bool OnlyFiles { get; set; }
    }
}
