using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetForEditQuery : IRequest<ChatRoomForEditResponse>
    {
        public long? Id { get; set; }
    }
}
