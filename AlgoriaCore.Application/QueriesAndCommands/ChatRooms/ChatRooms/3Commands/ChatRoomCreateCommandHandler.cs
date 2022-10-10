using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChatRooms;
using AlgoriaCore.Application.Managers.ChatRooms.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomCreateCommandHandler : BaseCoreClass, IRequestHandler<ChatRoomCreateCommand, long>
    {
        private readonly ChatRoomManager _manager;

        public ChatRoomCreateCommandHandler(ICoreServices coreServices, ChatRoomManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(ChatRoomCreateCommand request, CancellationToken cancellationToken)
        {
            ChatRoomDto dto = new ChatRoomDto()
            {
                ChatRoomId = request.ChatRoomId,
                Name = request.Name,
                Description = request.Description
            };

            return await _manager.CreateChatRoomAsync(dto);
        }
    }
}
