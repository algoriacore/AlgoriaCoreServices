using AlgoriaCore.Domain.Interfaces.Excel.Auditing;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Interfaces.Excel
{
    public interface IExcelService
    {
        IFileExcel ExportAuditLogsToFile(IAuditLogFilterExcel input, List<IAuditLogExcel> auditLogList);
        IFileExcel ExportAuditLogsToBinary(IAuditLogFilterExcel input, List<IAuditLogExcel> auditLogList);
    }
}
