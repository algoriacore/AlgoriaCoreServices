using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetOrCreateCommand : IRequest<ChatRoomResponse>
    {
        public string ChatRoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}