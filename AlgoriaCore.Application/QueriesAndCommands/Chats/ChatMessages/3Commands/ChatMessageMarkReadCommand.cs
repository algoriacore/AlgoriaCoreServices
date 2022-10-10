using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.ChatMessages._3Commands
{
    public class ChatMessageMarkReadCommand : IRequest<long>
    {
        public int? FriendTenantId { get; set; }
        public long FriendUserId { get; set; }
    }
}
