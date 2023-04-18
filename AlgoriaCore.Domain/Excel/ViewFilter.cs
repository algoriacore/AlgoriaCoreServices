using AlgoriaCore.Domain.Interfaces.Excel;

namespace AlgoriaCore.Domain.Excel
{
    public class ViewFilter : IViewFilter
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public object Value { get; set; }
    }
}
