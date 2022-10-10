using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.Languages.Dto
{
    public class GetLanguagesDto
    {
        public string DefaultLanguageName { get; set; }
        public List<LanguageDto> Items { get; set; }

        public GetLanguagesDto(List<LanguageDto> items, string defaultLanguageName)
        {
            DefaultLanguageName = defaultLanguageName;
            Items = items;
        }
    }
}
