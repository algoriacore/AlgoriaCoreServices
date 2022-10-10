using AlgoriaCore.Application.BaseClases.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageForEditResponse
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }

        public LanguageForEditResponse()
        {
            LanguageCombo = new List<ComboboxItemDto>();
        }

        public List<ComboboxItemDto> LanguageCombo { get; set; }
    }
}