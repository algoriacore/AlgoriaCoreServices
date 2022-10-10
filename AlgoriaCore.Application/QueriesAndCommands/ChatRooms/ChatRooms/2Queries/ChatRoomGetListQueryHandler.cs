using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.ChatRooms;
using AlgoriaCore.Application.Managers.ChatRooms.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetListQueryHandler : BaseCoreClass, IRequestHandler<ChatRoomGetListQuery, PagedResultDto<ChatRoomForListResponse>>
    {
        private readonly ChatRoomManager _managerChatRoom;

        public ChatRoomGetListQueryHandler(ICoreServices coreServices, ChatRoomManager managerChatRoom): base(coreServices)
        {
            _managerChatRoom = managerChatRoom;
        }

        public async Task<PagedResultDto<ChatRoomForListResponse>> Handle(ChatRoomGetListQuery request, CancellationToken cancellationToken)
        {
            ChatRoomListFilterDto filterDto = new ChatRoomListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            PagedResultDto<ChatRoomDto> pagedResultDto = await _managerChatRoom.GetChatRoomListAsync(filterDto);
            List<ChatRoomForListResponse> ll = new List<ChatRoomForListResponse>();

            foreach (ChatRoomDto dto in pagedResultDto.Items)
            {
                ll.Add(new ChatRoomForListResponse()
                {
                    Id = dto.Id.Value,
                    ChatRoomId = dto.ChatRoomId,
                    Name = dto.Name,
                    Description = dto.Description,
                    UserCreatorDesc = dto.UserCreatorDesc,
                    CreationTime = dto.CreationTime
                });
            }
            return new PagedResultDto<ChatRoomForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
