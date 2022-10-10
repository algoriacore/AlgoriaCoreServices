using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._3Commands
{
    public class FriendshipUnblockCommand : IRequest<long>
    {
        public int? FriendTenantId { get; set; }
        public long FriendUserId { get; set; }
    }
}
