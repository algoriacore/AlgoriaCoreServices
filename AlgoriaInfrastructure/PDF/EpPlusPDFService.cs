using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.Folder;
using AlgoriaCore.Domain.Interfaces.PDF;
using AlgoriaCore.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using static iTextSharp.text.pdf.AcroFields;

namespace AlgoriaInfrastructure.Excel
{
    public class EpPlusPDFService : IPDFService
    {
        private readonly IAppFolders _appFolders;
        private readonly IAppLocalizationProvider _appLocalizationProvider;

        public EpPlusPDFService(IAppFolders appFolders, IAppLocalizationProvider appLocalizationProvider)
        {
            _appFolders = appFolders;
            _appLocalizationProvider = appLocalizationProvider;
        }

        public byte[] ExportView(string title, List<ExpandoObject> list, List<IViewColumn> viewColumns)
        {
            PdfReport report = CreateBaseReport(title);
            IDictionary<string, object> propertyValues = list.FirstOrDefault();
            int order = 0;
            string propertyName;

            report.MainTableDataSource(dataSource =>
            {
                dataSource.DynamicData(list);
            })
            .MainTableColumns(columns =>
            {
                if (propertyValues != null)
                {
                    foreach (IViewColumn viewColumn in viewColumns.FindAll(p => p.IsActive))
                    {
                        order += 1;
                        propertyName = propertyValues.First(p => p.Key.ToLower() == viewColumn.Field.ToLower()).Key;

                        columns.AddColumn(column =>
                        {
                            column.PropertyName(propertyName);
                            column.CellsHorizontalAlignment(HorizontalAlignment.Left);
                            column.IsVisible(true);
                            column.Order(order);
                            column.Width(1);
                            column.HeaderCell(viewColumn.Header, horizontalAlignment: HorizontalAlignment.Left);
                        });
                    }
                }
            });

            return report.GenerateAsByteArray();
        }

        #region Métodos privados

        private PdfReport CreateBaseReport(string title)
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Landscape);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Algoria", Application = "Algoria Core", Keywords = "Report", Subject = title, Title = title });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });
            })
                .DefaultFonts(fonts =>
                {
                    fonts.Size(9);
                    fonts.Color(System.Drawing.Color.Black);
                })
                .PagesHeader(header =>
                {
                    header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                    header.DefaultHeader(defaultHeader =>
                    {
                        defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                        defaultHeader.Message(title);
                    });
                })
                .MainTableTemplate(template =>
                {
                    template.BasicTemplate(BasicTemplate.ClassicTemplate);
                })
                .MainTablePreferences(table =>
                {
                    table.ColumnsWidthsType(TableColumnWidthType.Relative);
                })
                .MainTableEvents(events =>
                {
                    events.DataSourceIsEmpty(message: L("RecordsNotFound"));
                });
        }

        private string L(string key)
        {
            return _appLocalizationProvider.L(key);
        }

        #endregion
    }
}