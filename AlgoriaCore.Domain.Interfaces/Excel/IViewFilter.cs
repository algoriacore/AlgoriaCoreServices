namespace AlgoriaCore.Domain.Interfaces.Excel
{
    public interface IViewFilter
    {
        string Name { get; set; }
        string Title { get; set; }
        object Value { get; set; }
    }
}
