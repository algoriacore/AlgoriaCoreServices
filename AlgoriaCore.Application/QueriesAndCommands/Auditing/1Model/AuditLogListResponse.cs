using System;

namespace AlgoriaCore.Application.QueriesAndCommands.Auditing._1Model
{
    public class AuditLogListResponse
    {
        public long? Id { get; set; }

        public string TenantName { get; set; }

        public long? UserId { get; set; }

        public string UserName { get; set; }

        public long? ImpersonalizerUserId { get; set; }

        public string ImpersonalizerUserName { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string Parameters { get; set; }

        public DateTime? ExecutionTime { get; set; }

        public int? ExecutionDuration { get; set; }

        public string ClientIpAddress { get; set; }

        public string ClientName { get; set; }

        public string BrowserInfo { get; set; }

        public string Exception { get; set; }

        public string CustomData { get; set; }

        public short? Severity { get; set; }
    }
}
