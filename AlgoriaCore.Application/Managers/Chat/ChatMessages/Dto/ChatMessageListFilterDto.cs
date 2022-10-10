namespace AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto
{
    public class ChatMessageListFilterDto
    {
        public int? TenantId { get; set; }

        public long UserId { get; set; }

        public long? MinMessageId { get; set; }
    }
}
