using AlgoriaCore.Domain.Interfaces.Excel;
using System.Collections.Generic;
using System.Dynamic;

namespace AlgoriaCore.Domain.Interfaces.PDF
{
    public interface IPDFService
    {
        byte[] ExportView(string title, List<ExpandoObject> list, List<IViewColumn> viewColumns);
    }
}
