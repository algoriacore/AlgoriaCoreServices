using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetByIdQuery : IRequest<ChatRoomResponse>
    {
        public long Id { get; set; }
    }
}
