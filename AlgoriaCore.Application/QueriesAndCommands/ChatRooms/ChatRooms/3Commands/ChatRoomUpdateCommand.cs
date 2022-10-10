using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomUpdateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}