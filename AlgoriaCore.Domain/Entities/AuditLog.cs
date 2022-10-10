using AlgoriaCore.Domain.Entities.Base;
using AlgoriaCore.Domain.Interfaces;
using System;

namespace AlgoriaCore.Domain.Entities
{
    public partial class AuditLog : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long? UserId { get; set; }
        public long? ImpersonalizerUserId { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public string Parameters { get; set; }
        public DateTime? ExecutionDatetime { get; set; }
        public int? ExecutionDuration { get; set; }
        public string ClientIpAddress { get; set; }
        public string ClientName { get; set; }
        public string BroserInfo { get; set; }
        public string Exception { get; set; }
        public string CustomData { get; set; }
        public byte? Severity { get; set; }

        public virtual User ImpersonalizerUser { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual User User { get; set; }
    }
}
