using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.Folders;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Excel;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.Excel.Auditing;
using AlgoriaCore.Domain.Interfaces.Folder;
using AlgoriaCore.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AlgoriaInfrastructure.Excel
{
    public class EpPlusExcelService : IExcelService
    {
        private readonly IAppFolders _appFolders;
        private readonly IAppLocalizationProvider _appLocalizationProvider;

        public EpPlusExcelService(IAppFolders appFolders, IAppLocalizationProvider appLocalizationProvider)
        {
            _appFolders = appFolders;
            _appLocalizationProvider = appLocalizationProvider;
        }

        public IFileExcel ExportAuditLogsToFile(IAuditLogFilterExcel input, List<IAuditLogExcel> auditLogList)
        {
            return CreateExcelPackage("AuditLogs.xlsx", GetActionCreatorAuditLogs(input, auditLogList), false);
        }

        public IFileExcel ExportAuditLogsToBinary(IAuditLogFilterExcel input, List<IAuditLogExcel> auditLogList)
        {
            return CreateExcelPackage("AuditLogs.xlsx", GetActionCreatorAuditLogs(input, auditLogList), true);
        }

		#region Métodos privados

		private Action<ExcelPackage> GetActionCreatorAuditLogs(IAuditLogFilterExcel input, List<IAuditLogExcel> auditLogList)
		{
			return excelPackage =>
			{
				var sheet = excelPackage.Workbook.Worksheets.Add(L("AuditLogs"));
				sheet.OutLineApplyStyle = true;

				var severity = L("AuditLogs.All");
				if (input.Severity.HasValue)
				{
					severity = GetSeverity(input.Severity);
				}

				AddTitle(sheet, 1, 1, 10, L("AuditLogs"));

				AddFilter(sheet, 2, 1, L("AuditLogs.DateRange"), input.StartDate.ToString("dd/MM/yyyy") + " - " + input.EndDate.ToString("dd/MM/yyyy"));
				AddFilter(sheet, 2, 3, L("AuditLogs.UserName"), input.UserName ?? string.Empty);
				AddFilter(sheet, 3, 1, L("AuditLogs.Service"), input.ServiceName ?? string.Empty);
				AddFilter(sheet, 3, 3, L("AuditLogs.Duration"), (input.MinExecutionDuration.HasValue && input.MaxExecutionDuration.HasValue ? input.MinExecutionDuration.ToString() + " - " + input.MaxExecutionDuration.ToString() : string.Empty));
				AddFilter(sheet, 4, 1, L("AuditLogs.Action"), input.MethodName ?? string.Empty);
				AddFilter(sheet, 4, 3, L("AuditLogs.ErrorState"), severity);
				AddFilter(sheet, 5, 1, L("AuditLogs.Browser"), input.BrowserInfo ?? string.Empty);

				sheet.Cells[2, 8].Value = DateTime.Now;
				sheet.Cells[2, 8].Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
				sheet.Cells[2, 8, 2, 10].Merge = true;
				sheet.Cells[2, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

				AddHeader(
					sheet, 6, 1,
					L("AuditLogs.ExecutionTimeColGrid"),
					L("AuditLogs.UserNameColGrid"),
					L("AuditLogs.ImpersonalizerUserNameColGrid"),
					L("AuditLogs.ServiceColGrid"),
					L("AuditLogs.MethodColGrid"),
					L("AuditLogs.ParametersColGrid"),
					L("AuditLogs.ExecutionDurationColGrid"),
					L("AuditLogs.ClientIpAddressColGrid"),
					L("AuditLogs.ClientNameColGrid"),
					L("AuditLogs.BrowserColGrid"),
					L("AuditLogs.ErrorStateColGrid"),
					L("AuditLogs.Severity")
				);

				AddObjects(
					sheet, 7, auditLogList,
					//_ => _timeZoneConverter.Convert(_.ExecutionTime, _abpSession.TenantId, _abpSession.GetUserId()),
					_ => _.ExecutionTime,
					_ => _.UserName,
					_ => _.ImpersonalizerUserName,
					_ => _.ServiceName,
					_ => _.MethodName,
					_ => _.Parameters,
					_ => _.ExecutionDuration,
					_ => _.ClientIpAddress,
					_ => _.ClientName,
					_ => _.BrowserInfo,
					_ => _.Exception.IsNullOrEmpty() ? string.Empty : _.Exception,
					_ => GetSeverity(_.Severity)
					);

				//Formatting cells

				var timeColumn = sheet.Column(1);
				timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

				for (var i = 1; i <= 12; i++)
				{
					if (i.IsIn(5, 10)) //Don't AutoFit Parameters and Exception
					{
						continue;
					}

					sheet.Column(i).AutoFit();
				}
			};
		}

		private string GetSeverity(short? severity)
		{
			string resp = string.Empty;
			switch (severity)
			{
				case 0: resp = L("LogLevelTrace"); break;
				case 1: resp = L("LogLevelDebug"); break;
				case 2: resp = L("LogLevelInformation"); break;
				case 3: resp = L("LogLevelWarning"); break;
				case 4: resp = L("LogLevelError"); break;
				case 5: resp = L("LogLevelCritical"); break;
			}

			return resp;
		}

        private FileExcel CreateExcelPackage(string fileName, Action<ExcelPackage> creator, bool isBinary)
        {
            var file = new FileExcel(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (var excelPackage = new ExcelPackage())
            {
                creator(excelPackage);

                if (isBinary)
                {
                    file.FileArray = excelPackage.GetAsByteArray();
                }
                else
                {
                    Save(excelPackage, file);
                }
            }

            return file;
        }

        private FileExcel CreateExcelPackageFromTemplate(string pathTemplate, string fileName, Action<ExcelPackage> creator, bool isBinary)
        {
            var file = new FileExcel(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (var excelPackage = new ExcelPackage(new FileInfo(pathTemplate)))
            {
                creator(excelPackage);

                if (isBinary)
                {
                    file.FileArray = excelPackage.GetAsByteArray();
                }
                else
                {
                    Save(excelPackage, file);
                }
            }

            return file;
        }

        private void AddHeader(ExcelWorksheet sheet, int rowIndex, int columnIndex, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, rowIndex, columnIndex + i, headerTexts[i]);
            }
        }

        private void AddHeader(ExcelWorksheet sheet, int rowIndex, int columnIndex, string headerText)
        {
            sheet.Cells[rowIndex, columnIndex].Value = headerText;
            sheet.Cells[rowIndex, columnIndex].Style.Font.Bold = true;
            sheet.Cells[rowIndex, columnIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[rowIndex, columnIndex].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        }

        private void AddTitle(ExcelWorksheet sheet, int rowIndex, int columnIndex, int toColumnIndex, string titleText)
        {
            sheet.Cells[rowIndex, columnIndex, rowIndex, toColumnIndex].Merge = true;
            sheet.Cells[rowIndex, columnIndex].Value = titleText;
            sheet.Cells[rowIndex, columnIndex].Style.Font.Bold = true;
            sheet.Cells[rowIndex, columnIndex].Style.Font.Size = 16.0f;
        }

        private void AddFilter(ExcelWorksheet sheet, int rowIndex, int columnIndex, string filterText, object filterValueText)
        {
            AddFilter(sheet, rowIndex, columnIndex, filterText, filterValueText, null);
        }

        private void AddFilter(ExcelWorksheet sheet, int rowIndex, int columnIndex, string filterText, object filterValueText, string format)
        {
            sheet.Cells[rowIndex, columnIndex].Value = filterText;
            sheet.Cells[rowIndex, columnIndex].Style.Font.Bold = true;

            sheet.Cells[rowIndex, columnIndex + 1].Value = filterValueText;
            sheet.Cells[rowIndex, columnIndex + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            if (!format.IsNullOrEmpty())
            {
                sheet.Cells[rowIndex, columnIndex + 1].Style.Numberformat.Format = format;
            }
        }

        private void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, int startColIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + startColIndex].Value = propertySelectors[j](items[i]);
                }
            }
        }

        private void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
                }
            }
        }

        private void Save(ExcelPackage excelPackage, FileExcel file)
        {
            var filePath = Path.Combine(_appFolders.TempFileDownloadFolder, file.FileToken);
            excelPackage.SaveAs(new FileInfo(filePath));
        }

        private string L(string key)
        {
            return _appLocalizationProvider.L(key);
        }

        #endregion
    }
}