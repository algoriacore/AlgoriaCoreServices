using AlgoriaCore.Application.Managers.Chat.Friendships.Dto;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._1Model
{
    public class FriendshipListResponse
    {
        public long Id { get; set; }

        public long FriendUserId { get; set; }

        public int? FriendTenantId { get; set; }

        public string FriendUserName { get; set; }

        public string FriendTenancyName { get; set; }

        public string FriendProfilePictureUrl { get; set; }

        public int UnreadMessageCount { get; set; }

        public bool IsOnline { get; set; }

        public FriendshipState State { get; set; }
    }
}
