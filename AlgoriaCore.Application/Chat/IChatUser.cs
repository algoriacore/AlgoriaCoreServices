namespace AlgoriaCore.Application.Chat
{
    public interface IChatUser
    {
        long UserId { get; set; }

        int? TenantId { get; set; }
    }
}
