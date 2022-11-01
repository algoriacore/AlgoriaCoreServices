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
    public class ChatRoomChatGetListQueryHandler : BaseCoreClass, IRequestHandler<ChatRoomChatGetListQuery, PagedResultDto<ChatRoomChatForListResponse>>
    {
        private readonly ChatRoomManager _manager;

        public ChatRoomChatGetListQueryHandler(ICoreServices coreServices, ChatRoomManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<ChatRoomChatForListResponse>> Handle(ChatRoomChatGetListQuery request, CancellationToken cancellationToken)
        {
            ChatRoomChatListFilterDto filterDto = new ChatRoomChatListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                ChatRoom = request.ChatRoom,
                ChatRoomId = request.ChatRoomId,
                Skip = request.Skip,
                OnlyFiles = request.OnlyFiles
            };

            PagedResultDto<ChatRoomChatDto> pagedResultDto = await _manager.GetChatRoomChatListAsync(filterDto);
            List<ChatRoomChatForListResponse> ll = new List<ChatRoomChatForListResponse>();

            foreach (ChatRoomChatDto dto in pagedResultDto.Items)
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
            return new PagedResultDto<ChatRoomChatForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
