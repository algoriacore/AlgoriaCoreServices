namespace AlgoriaCore.Domain.Interfaces.Entity
{
    public interface IPagedResult
    {
        int? PageSize { get; set; }
        int? PageNumber { get; set; }
    }
}
