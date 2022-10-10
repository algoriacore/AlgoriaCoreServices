using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChatRooms;
using AlgoriaCore.Application.Managers.ChatRooms.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomUpdateCommandHandler : BaseCoreClass, IRequestHandler<ChatRoomUpdateCommand, long>
    {
        private readonly ChatRoomManager _managerChatRoom;

        public ChatRoomUpdateCommandHandler(ICoreServices coreServices, ChatRoomManager managerChatRoom): base(coreServices)
        {
            _managerChatRoom = managerChatRoom;
        }

        public async Task<long> Handle(ChatRoomUpdateCommand request, CancellationToken cancellationToken)
        {
            ChatRoomDto dto = new ChatRoomDto()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description
            };

            await _managerChatRoom.UpdateChatRoomAsync(dto);

            return dto.Id.Value;
        }
    }
}
