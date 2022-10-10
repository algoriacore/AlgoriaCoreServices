namespace AlgoriaCore.Domain.Session
{
    public interface IAppSession
    {
        int? TenantId { get; set; }

        string TenancyName { get; set; }

        long? UserId { get; set; }

        string UserName { get; set; }

        bool IsImpersonalized { get; set; }

        long? ImpersonalizerUserId { get; set; }

        string TimeZone { get; set; }
    }
}
