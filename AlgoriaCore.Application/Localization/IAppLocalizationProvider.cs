namespace AlgoriaCore.Application.Localization
{
    public interface IAppLocalizationProvider
    {
        string L(string key);
        string L(string key, params string[] parameters);
    }
}
