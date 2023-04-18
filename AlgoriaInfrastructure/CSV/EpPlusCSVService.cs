using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Interfaces.CSV;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.Folder;
using AlgoriaCore.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgoriaInfrastructure.Excel
{
    public class EpPlusCSVService : ICSVService
    {
        private readonly IAppFolders _appFolders;
        private readonly IAppLocalizationProvider _appLocalizationProvider;

        public EpPlusCSVService(IAppFolders appFolders, IAppLocalizationProvider appLocalizationProvider)
        {
            _appFolders = appFolders;
            _appLocalizationProvider = appLocalizationProvider;
        }

        public byte[] ExportView(List<ExpandoObject> list, List<IViewColumn> viewColumns)
        {
            var excelPackage = new ExcelPackage();
            var sheet = excelPackage.Workbook.Worksheets.Add(L("Users"));

            List<IViewColumn> viewColumnsActive = viewColumns.FindAll(p => p.IsActive);
            List<Func<ExpandoObject, object>> parameters = new List<Func<ExpandoObject, object>>();

            foreach (IViewColumn viewColumn in viewColumnsActive)
            {
                parameters.Add(_ => _.First(p => p.Key.ToLower() == viewColumn.Field.ToLower()).Value);
            }

            AddHeader(sheet, 1, 1, viewColumnsActive.Select(p => p.Header).ToArray());
            AddObjects(sheet, 2, list, parameters.ToArray());

            var range = sheet.Cells[1, 1, list.Count() + 1, viewColumnsActive.Count()];
            var stream = new MemoryStream();

            range.SaveToText(stream, new ExcelOutputTextFormat { Encoding = Encoding.UTF8, TextQualifier = '"' });

            return stream.ToArray();
        }

        #region Métodos privados

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

        private string L(string key)
        {
            return _appLocalizationProvider.L(key);
        }

        #endregion
    }
}