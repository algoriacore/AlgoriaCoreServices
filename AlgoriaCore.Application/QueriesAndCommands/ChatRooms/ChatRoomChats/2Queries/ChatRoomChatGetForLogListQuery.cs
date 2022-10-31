using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats
{
    public class ChatRoomChatGetForLogListQuery : IRequest<List<ChatRoomChatForListResponse>>
    {
        public long? LastId { get; set; }
    }
}
