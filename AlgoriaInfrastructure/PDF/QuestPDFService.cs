using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.Folder;
using AlgoriaCore.Domain.Interfaces.PDF;
using AlgoriaCore.Extensions.Collections;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaInfrastructure.Excel
{
    public class QuestPDFService : IPDFService
    {
        private readonly IAppFolders _appFolders;
        private readonly IAppLocalizationProvider _appLocalizationProvider;

        public QuestPDFService(IAppFolders appFolders, IAppLocalizationProvider appLocalizationProvider)
        {
            _appFolders = appFolders;
            _appLocalizationProvider = appLocalizationProvider;
        }

        public async Task<byte[]> ExportView(string title, List<ExpandoObject> list, List<IViewColumn> viewColumns, List<IViewFilter> viewFilters)
        {
            var activeColumns = viewColumns.FindAll(p => p.IsActive);

            QuestPDF.Settings.DocumentLayoutExceptionThreshold = 5000;

            byte[] bytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.MarginHorizontal(1f, Unit.Centimetre);
                    page.MarginVertical(1f, Unit.Centimetre);

                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text(title);
                        col.Item().DefaultTextStyle(TextStyle.Default.LineHeight(0.5f)).Text(text => text.Line(""));

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            uint j = 1;

                            if (viewFilters.Count > 0)
                            {
                                List<IViewFilter> viewFiltersAux = viewFilters.FindAll(p => p.Name != "Filter");
                                uint i;

                                foreach (var chunk in viewFiltersAux.Chunk(3))
                                {
                                    i = 1;

                                    foreach (var p in chunk)
                                    {
                                        table.Cell().Row(j).Column(i).DefaultTextStyle(TextStyle.Default.Weight(FontWeight.Medium)).Text(p.Title);
                                        table.Cell().Row(j).Column(++i).Text(p.Value == null ? "" : p.Value.ToString());

                                        i++;
                                    }

                                    j++;
                                }

                                IViewFilter viewFilterSearch = viewFilters.FirstOrDefault(p => p.Name == "Filter");

                                if (viewFilterSearch != null)
                                {
                                    table.Cell().Row(j).Column(1).DefaultTextStyle(TextStyle.Default.Weight(FontWeight.Medium)).Text(viewFilterSearch.Title);
                                    table.Cell().Row(j).Column(2).ColumnSpan(5).Text(viewFilterSearch.Value == null ? "" : viewFilterSearch.Value.ToString());
                                }
                            }
                        });

                        col.Item().DefaultTextStyle(TextStyle.Default.LineHeight(0.5f)).Text(text => text.Line(""));
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var viewColumn in activeColumns)
                                {
                                    columns.RelativeColumn();
                                }
                            });

                            uint i = 1;
                            uint j = 1;

                            foreach (var viewColumn in activeColumns)
                            {
                                table.Cell().Row(j).Column(i).BorderBottom(0.5f, Unit.Point).BorderColor("#E9ECEF").Background("#F4F4F4").Text(viewColumn.Header);
                                i++;
                            }

                            j++;

                            if (list.Count > 0)
                            {
                                IDictionary<string, object> propertyValues = list.FirstOrDefault();
                                IDictionary<string, string> propertyNames = new Dictionary<string, string>();
                                string propertyName;

                                foreach (var viewColumn in activeColumns)
                                {
                                    propertyName = propertyValues.First(p => p.Key.ToLower() == viewColumn.Field.ToLower()).Key;
                                    propertyNames.Add(viewColumn.Field, propertyName);
                                }

                                object value;
                                string bg;

                                foreach (ExpandoObject item in list)
                                {
                                    i = 1;

                                    foreach (var viewColumn in activeColumns)
                                    {
                                        propertyName = propertyNames.GetOrDefault(viewColumn.Field);
                                        value = item.GetOrDefault(propertyName);

                                        bg = j % 2 == 0 ? "#FFFFFF" : "#F4F4F4";

                                        table.Cell().Row(j).Column(i).BorderBottom(0.5f, Unit.Point).BorderColor("#E9ECEF").Background(bg)
                                        .Text(value == null ? "" : value.ToString());

                                        i++;
                                    }

                                    j++;
                                }
                            }
                            else
                            {
                                table.Cell().Row(j).ColumnSpan((uint)activeColumns.Count).BorderBottom(0.5f, Unit.Point).BorderColor("#E9ECEF").AlignCenter().Text(L("RecordsNotFound"));
                            }
                        });
                    });
                });
            }).GeneratePdf();

            return await Task.FromResult(bytes);
        }

        private string L(string key)
        {
            return _appLocalizationProvider.L(key);
        }
    }
}