using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChatRooms;
using AlgoriaCore.Application.Managers.ChatRooms.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetByIdQueryHandler : BaseCoreClass, IRequestHandler<ChatRoomGetByIdQuery, ChatRoomResponse>
    {
        private readonly ChatRoomManager _manager;

        public ChatRoomGetByIdQueryHandler(ICoreServices coreServices, ChatRoomManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<ChatRoomResponse> Handle(ChatRoomGetByIdQuery request, CancellationToken cancellationToken)
        {
            ChatRoomResponse response = null;
            ChatRoomDto dto = await _manager.GetChatRoomAsync(request.Id);

            response = new ChatRoomResponse()
            {
                Id = dto.Id.Value,
                UserCreator = dto.UserCreator,
                UserCreatorDesc = dto.UserCreatorDesc,
                ChatRoomId = dto.ChatRoomId,
                Name = dto.Name,
                Description = dto.Description,
                CreationTime = dto.CreationTime
            };

            return response;
        }
    }
}
