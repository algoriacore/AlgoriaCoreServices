namespace AlgoriaCore.Domain.Interfaces
{
    public interface IMayHaveTenant
    {
        int? TenantId { get; set; }
    }
}
