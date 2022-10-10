using AlgoriaCore.Application.Managers.Chat.Friendships.Dto;

namespace AlgoriaCore.Application.Chat
{
    public class ChatFriendData
    {
        public long? Id { get; set; }
        public int? FriendTenantId { get; set; }
        public long FriendUserId { get; set; }
        public string FriendNickname { get; set; }
        public string FriendTenancyName { get; set; }
        public string FriendProfilePictureUrl { get; set; }
        public int UnreadMessageCount { get; set; }
        public FriendshipState State { get; set; }
        public bool IsOnline { get; set; }
    }
}
