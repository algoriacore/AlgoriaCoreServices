using System;

namespace AlgoriaCore.Domain.Interfaces.Excel.Auditing
{
    public interface IAuditLogFilterExcel
    {
        DateTime StartDate { get; set; }

        DateTime EndDate { get; set; }

        string UserName { get; set; }

        string ServiceName { get; set; }

        string MethodName { get; set; }

        string BrowserInfo { get; set; }

        bool? HasException { get; set; }

        int? MinExecutionDuration { get; set; }

        int? MaxExecutionDuration { get; set; }

        short? Severity { get; set; }
    }
}
