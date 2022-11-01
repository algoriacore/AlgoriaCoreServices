using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChatRooms;
using AlgoriaCore.Application.Managers.ChatRooms.Dto;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats
{
    public class ChatRoomChatGetForLogListQueryHandler : BaseCoreClass, IRequestHandler<ChatRoomChatGetForLogListQuery, List<ChatRoomChatForListResponse>>
    {
        private readonly ChatRoomManager _manager;

        public ChatRoomChatGetForLogListQueryHandler(ICoreServices coreServices, ChatRoomManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ChatRoomChatForListResponse>> Handle(ChatRoomChatGetForLogListQuery request, CancellationToken cancellationToken)
        {
            ChatRoomChatForLogFilterDto filterDto = new ChatRoomChatForLogFilterDto()
            {
                ChatRoomId = request.ChatRoomId,
                LastId = request.LastId ?? 0
            };

            List<ChatRoomChatDto> lst = await _manager.GetChatRoomChatListForLogAsync(filterDto);
            List<ChatRoomChatForListResponse> ll = new List<ChatRoomChatForListResponse>();

            foreach (ChatRoomChatDto dto in lst)
            {
                ll.Add(new ChatRoomChatForListResponse()
                {
                    Id = dto.Id.Value,
                    ChatRoom = dto.ChatRoom,
                    User = dto.User,
                    UserDesc = dto.UserDesc,
                    UserLogin = dto.UserLogin,
                    CreationTime = dto.CreationTime,
                    Comment = dto.Comment,
                    Files = dto.Files.Select(p => new ChatRoomChatFileResponse()
                    {
                        FileName = p.FileName,
                        FileExtension = p.FileExtension,
                        File = p.File
                    }).ToList()
                });
            }

            return ll;
        }
    }
}
