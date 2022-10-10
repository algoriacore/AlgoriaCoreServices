using AlgoriaCore.Application.Chat;

namespace AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto
{
    public class ChatUser : IChatUser
    {
        public long UserId { get; set; }

        public int? TenantId { get; set; }

        public ChatUser(int? TenantId, long UserId)
        {
            this.TenantId = TenantId;
            this.UserId = UserId;
        }

        public override string ToString()
        {
            if (!TenantId.HasValue)
            {
                return UserId.ToString();
            }

            return UserId + "@" + TenantId;
        }
    }
}
