namespace AlgoriaCore.Domain.Interfaces.Excel
{
    public interface IViewColumn
    {
        string Field { get; set; }
        string Header { get; set; }
        string Format { get; set; }
        bool IsActive { get; set; }
    }
}
