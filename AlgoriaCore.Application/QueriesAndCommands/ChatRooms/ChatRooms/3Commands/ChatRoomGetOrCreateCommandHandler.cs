using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChatRooms;
using AlgoriaCore.Application.Managers.ChatRooms.Dto;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetOrCreateCommandHandler : BaseCoreClass, IRequestHandler<ChatRoomGetOrCreateCommand, ChatRoomResponse>
    {
        private readonly ChatRoomManager _manager;

        public ChatRoomGetOrCreateCommandHandler(ICoreServices coreServices, ChatRoomManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<ChatRoomResponse> Handle(ChatRoomGetOrCreateCommand request, CancellationToken cancellationToken)
        {
            ChatRoomDto dto = (await _manager.GetChatRoomByChatRoomIdListAsync(request.ChatRoomId)).FirstOrDefault();

            if (dto == null)
            {
                dto = new ChatRoomDto()
                {
                    ChatRoomId = request.ChatRoomId,
                    Name = request.Name,
                    Description = request.Description
                };

                dto.Id = await _manager.CreateChatRoomAsync(dto);
                dto = await _manager.GetChatRoomAsync(dto.Id.Value);
            }

            return new ChatRoomResponse()
            {
                Id = dto.Id.Value,
                UserCreator = dto.UserCreator,
                UserCreatorDesc = dto.UserCreatorDesc,
                ChatRoomId = dto.ChatRoomId,
                Name = dto.Name,
                Description = dto.Description,
                CreationTime = dto.CreationTime
            };
        }
    }
}
