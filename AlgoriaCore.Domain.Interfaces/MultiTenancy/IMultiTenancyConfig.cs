namespace AlgoriaCore.Domain.Interfaces.MultiTenancy
{
    public interface IMultiTenancyConfig
    {
        bool IsEnabled();
        string GetTenancyNameDefault();
    }
}
