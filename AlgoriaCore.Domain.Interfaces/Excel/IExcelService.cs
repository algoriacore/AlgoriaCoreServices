using AlgoriaCore.Domain.Interfaces.Excel.Auditing;
using System.Collections.Generic;
using System.Dynamic;

namespace AlgoriaCore.Domain.Interfaces.Excel
{
    public interface IExcelService
    {
        IFileExcel ExportAuditLogsToFile(IAuditLogFilterExcel input, List<IAuditLogExcel> auditLogList);
        IFileExcel ExportAuditLogsToBinary(IAuditLogFilterExcel input, List<IAuditLogExcel> auditLogList);
        IFileExcel ExportView(string filter, List<ExpandoObject> list, List<IViewColumn> viewColumns);
    }
}
