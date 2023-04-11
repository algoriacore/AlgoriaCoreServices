using AlgoriaCore.Domain.Interfaces.Excel;

namespace AlgoriaCore.Domain.Excel
{
    public class ViewColumn: IViewColumn
    {
        public string Field { get; set; }
        public string Header { get; set; }
        public string Format { get; set; }
        public bool IsActive { get; set; }
    }
}
