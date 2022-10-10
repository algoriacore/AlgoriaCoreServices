using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageListResponse
    {
        public string DefaultLanguageName { get; set; }
        public List<LanguageForListResponse> Items { get; set; }

        public LanguageListResponse(List<LanguageForListResponse> items, string defaultLanguageName)
        {
            DefaultLanguageName = defaultLanguageName;
            Items = items;
        }
    }
}