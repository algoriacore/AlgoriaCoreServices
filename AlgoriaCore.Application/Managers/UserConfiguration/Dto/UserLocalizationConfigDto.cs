using AlgoriaCore.Application.Managers.Languages.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.UserConfiguration.Dto
{
    public class UserLocalizationConfigDto
    {
        public LanguageDto DefaultLanguage { get; set; }
        public LanguageDto CurrentLanguage { get; set; }
        public Dictionary<string, string> Values { get; set; }
    }
}
