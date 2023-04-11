using AlgoriaCore.Domain.Interfaces.Excel;
using System.Collections.Generic;
using System.Dynamic;

namespace AlgoriaCore.Domain.Interfaces.CSV
{
    public interface ICSVService
    {
        byte[] ExportViewUsersToBinary(List<ExpandoObject> list, List<IViewColumn> viewColumns);
    }
}
