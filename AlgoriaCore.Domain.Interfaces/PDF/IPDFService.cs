using AlgoriaCore.Domain.Interfaces.Excel;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace AlgoriaCore.Domain.Interfaces.PDF
{
    public interface IPDFService
    {
        Task<byte[]> ExportView(string title, List<ExpandoObject> list, List<IViewColumn> viewColumns, List<IViewFilter> viewFilters);
    }
}
