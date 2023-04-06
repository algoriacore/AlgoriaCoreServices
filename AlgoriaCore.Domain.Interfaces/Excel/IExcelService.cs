using AlgoriaCore.Domain.Interfaces.Excel.Auditing;
using AlgoriaCore.Domain.Interfaces.Excel.Users;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Interfaces.Excel
{
    public interface IExcelService
    {
        IFileExcel ExportAuditLogsToFile(IAuditLogFilterExcel input, List<IAuditLogExcel> auditLogList);
        IFileExcel ExportAuditLogsToBinary(IAuditLogFilterExcel input, List<IAuditLogExcel> auditLogList);
        IFileExcel ExportViewUsersToBinary(IUserFilterExcel input, List<IUserExcel> list, List<IViewColumn> viewColumns);
    }
}
