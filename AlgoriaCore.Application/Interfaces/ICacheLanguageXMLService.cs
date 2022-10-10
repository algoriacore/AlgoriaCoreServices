using AlgoriaCore.Application.Managers.Languages.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Interfaces
{
    public interface ICacheLanguageXmlService
    {
        string GenerateCacheKey(string languageCode, string keyLabel);
        string GetEntry(string languageCode, string keyLabel);
        string GetEntry(string keyLabel);
        List<LanguageTextDto> GetLanguageTextFromXML(string languageName = "", bool takeDefaultIsNotExists = true);
    }
}
