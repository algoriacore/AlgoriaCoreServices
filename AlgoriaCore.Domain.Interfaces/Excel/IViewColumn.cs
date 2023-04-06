namespace AlgoriaCore.Domain.Interfaces.Excel
{
    public interface IViewColumn
    {
        string Field { get; set; }
        string Header { get; set; }
        bool IsActive { get; set; }
    }
}
