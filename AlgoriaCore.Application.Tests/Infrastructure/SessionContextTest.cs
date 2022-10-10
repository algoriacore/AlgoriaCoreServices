using AlgoriaCore.Domain.Session;

namespace AlgoriaCore.Application.Tests.Infrastructure
{
    public class SessionContextTest: IAppSession
    {
        public int? TenantId { get; set; }
        public string TenancyName { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public bool IsImpersonalized { get; set; }
        public long? ImpersonalizerUserId { get; set; }
        public string TimeZone { get; set; }
    }
}
