namespace AlgoriaCore.Application.Managers.Chat.Friendships.Dto
{
    public class FriendshipBaseDto
    {
        public int? TenantId { get; set; }
        public long UserId { get; set; }
        public int? FriendTenantId { get; set; }
        public long FriendUserId { get; set; }
    }
}
