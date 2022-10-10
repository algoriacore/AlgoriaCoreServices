using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms
{
    public class ChatRoomGetListQuery : PageListByDto, IRequest<PagedResultDto<ChatRoomForListResponse>>
    {

    }
}
