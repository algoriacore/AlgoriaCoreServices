using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats
{
    public class ChatRoomChatGetByIdQuery : IRequest<ChatRoomChatResponse>
    {
        public long Id { get; set; }
    }
}
