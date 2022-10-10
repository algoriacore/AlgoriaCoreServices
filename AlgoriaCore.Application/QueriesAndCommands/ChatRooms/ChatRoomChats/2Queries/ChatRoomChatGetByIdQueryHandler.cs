using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChatRooms;
using AlgoriaCore.Application.Managers.ChatRooms.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats
{
    public class ChatRoomChatGetByIdQueryHandler : BaseCoreClass, IRequestHandler<ChatRoomChatGetByIdQuery, ChatRoomChatResponse>
    {
        private readonly ChatRoomManager _manager;

        public ChatRoomChatGetByIdQueryHandler(ICoreServices coreServices, ChatRoomManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<ChatRoomChatResponse> Handle(ChatRoomChatGetByIdQuery request, CancellationToken cancellationToken)
        {
            ChatRoomChatResponse response = null;
            ChatRoomChatDto dto = await _manager.GetChatRoomChatAsync(request.Id);

            response = new ChatRoomChatResponse()
            {
                Id = dto.Id.Value,
                ChatRoom = dto.ChatRoom,
                User = dto.User,
                UserDesc = dto.UserDesc,
                Comment = dto.Comment,
                CreationTime = dto.CreationTime
            };

            return response;
        }
    }
}
