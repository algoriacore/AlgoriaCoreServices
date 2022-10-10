using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetByChatRoomIdQuery : IRequest<ChatRoomResponse>
    {
        public string ChatRoomId { get; set; }
    }
}
