using System;

namespace AlgoriaCore.Domain.Interfaces.Excel.Auditing
{
    public interface IAuditLogExcel
    {
        long? Id { get; set; }
        string TenantName { get; set; }
        long? UserId { get; set; }
        string UserName { get; set; } 
        long? ImpersonalizerUserId { get; set; }
        string ImpersonalizerUserName { get; set; }
        string ServiceName { get; set; }
        string MethodName { get; set; }
        string Parameters { get; set; }
        DateTime? ExecutionTime { get; set; }
        int? ExecutionDuration { get; set; }
        string ClientIpAddress { get; set; }
        string ClientName { get; set; }
        string BrowserInfo { get; set; }
        string Exception { get; set; }
        string CustomData { get; set; }
		short? Severity { get; set; }
    }
}
