using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Chats.ChatMessages._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.ChatMessages._2Queries
{
    public class ChatMessageGetListQuery : IRequest<PagedResultDto<ChatMessageListResponse>>
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public long? MinMessageId { get; set; }
    }
}
