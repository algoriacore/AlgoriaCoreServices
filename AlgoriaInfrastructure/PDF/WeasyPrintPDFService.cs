using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.Folder;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Interfaces.PDF;
using AlgoriaCore.Extensions.Collections;
using Balbarak.WeasyPrint;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaInfrastructure.Excel
{
    public class WeasyPrintPDFService : IPDFService
    {
        private readonly IAppFolders _appFolders;
        private readonly IAppLocalizationProvider _appLocalizationProvider;

        private readonly ICoreLogger _coreLogger;

        public WeasyPrintPDFService(IAppFolders appFolders, IAppLocalizationProvider appLocalizationProvider, ICoreLogger coreLogger)
        {
            _appFolders = appFolders;
            _appLocalizationProvider = appLocalizationProvider;

            _coreLogger = coreLogger;
        }

        public async Task<byte[]> ExportView(string title, List<ExpandoObject> list, List<IViewColumn> viewColumns, List<IViewFilter> viewFilters)
        {
            var css = "<style>@page { size: landscape; margin: 1cm; } #tblFilters { width: 100%; } .label { font-weight: 600; }" +
                "#tblMain { width: 100%; } #tblMain thead tr { background-color: #F4F4F4; } #tblMain th { font-weight: 600; }" +
                "#tblMain tr { min-height: 0.5cm; } #tblMain tbody tr:nth-child(even) { background-color: #F4F4F4; }</style>";
            var titleHTML = $@"<center><h3>{title}</h3></center><br />";
            var filtersHTML = "";

            if (viewFilters.Count > 0)
            {
                List<IViewFilter> viewFiltersAux = viewFilters.FindAll(p => p.Name != "Filter");

                foreach (var chunk in viewFiltersAux.Chunk(3))
                {
                    filtersHTML += "<tr>";
                    filtersHTML += string.Join("", chunk.Select(p => $@"<td><span class='label'>{p.Title}</span></td><td>{p.Value}</td>"));
                    filtersHTML += "</tr>";
                }

                IViewFilter viewFilterSearch = viewFilters.FirstOrDefault(p => p.Name == "Filter");

                if (viewFilterSearch != null)
                {
                    filtersHTML += "<tr>";
                    filtersHTML += $@"<td width='{100 / 6}%'><span class='label'>{viewFilterSearch.Title}</span></td><td colspan='5'>{viewFilterSearch.Value}</td>";
                    filtersHTML += "</tr>";
                }

                filtersHTML = $@"<table id='tblFilters'>{filtersHTML}</table><br />";
            }

            var activeColumns = viewColumns.FindAll(p => p.IsActive);
            var mainTableHTML = new StringBuilder("<table id='tblMain'>");

            mainTableHTML.Append("<thead><tr>");
            mainTableHTML.Append(string.Join("", activeColumns.Select(p => $@"<th>{p.Header}</th>")));
            mainTableHTML.Append("</tr></thead><tbody>");

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

                foreach (ExpandoObject item in list)
                {
                    mainTableHTML.Append("<tr>");

                    foreach (var viewColumn in activeColumns)
                    {
                        propertyName = propertyNames.GetOrDefault(viewColumn.Field);

                        mainTableHTML.Append($@"<td>{item.GetOrDefault(propertyName)}</td>");
                    }

                    mainTableHTML.Append("</tr>");
                }
            } else 
            {
                mainTableHTML.Append($@"<tr><td colspan='{activeColumns.Count}'><center>{L("RecordsNotFound")}</center></td></tr>");
            }

            mainTableHTML.Append("</tbody></table>");

            var body = titleHTML + filtersHTML + mainTableHTML;
            var html = $@"<!DOCTYPE html><html><head>{css}</head><body>{body}</body></html>";

            using (WeasyPrintClient client = new WeasyPrintClient())
            {
            client.OnDataError += (OutputEventArgs e) => {
                _coreLogger.LogError("PDF Error: " + e.Data);
            };

                return await client.GeneratePdfAsync(html);
            }
        }

        private string L(string key)
        {
            return _appLocalizationProvider.L(key);
        }
    }
}