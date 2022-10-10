namespace AlgoriaCore.Application.Interfaces
{
    public interface ICacheLanguageService
    {
        string SetEntry(int? tenantId, int? language, string keyLabel, string value);
        string GetEntry(int? tenantId, int? language, string keyLabel);
        void RemoveEntry(int? tenantId, int? language, string keyLabel);
        void CancelEntryParentLanguage(int? tenantId, int? language);
    }
}
