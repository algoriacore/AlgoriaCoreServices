using AlgoriaCore.Domain.Interfaces.Excel.Auditing;
using System;

namespace AlgoriaCore.Domain.Excel.Auditing
{
    public class AuditLogFilterExcel : IAuditLogFilterExcel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string UserName { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string BrowserInfo { get; set; }

        public bool? HasException { get; set; }

        public int? MinExecutionDuration { get; set; }

        public int? MaxExecutionDuration { get; set; }

        public short? Severity { get; set; }
    }
}
