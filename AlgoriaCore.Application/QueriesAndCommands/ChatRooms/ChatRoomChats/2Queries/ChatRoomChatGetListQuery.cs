using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats
{
    public class ChatRoomChatGetListQuery : PageListByDto, IRequest<PagedResultDto<ChatRoomChatForListResponse>>
    {
        public long? ChatRoom { get; set; }
        public string ChatRoomId { get; set; }
        public int Skip { get; set; }
        public bool OnlyFiles { get; set; }
    }
}
