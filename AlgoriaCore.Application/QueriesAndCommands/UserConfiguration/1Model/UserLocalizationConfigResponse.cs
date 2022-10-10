using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.UserConfiguration
{
    public class UserLocalizationConfigResponse
    {
        public LanguageInfoResponse DefaultLanguage { get; set; }
        public LanguageInfoResponse CurrentLanguage { get; set; }
        public Dictionary<string, string> Values { get; set; }
    }
}