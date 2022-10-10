using AlgoriaCore.Application.BaseClases.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpForEditResponse
    {
        public long? Id { get; set; }
        public int? Language { get; set; }
        public string LanguageDesc { get; set; }
        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }

        public List<ComboboxItemDto> LanguageCombo { get; set; }
        public List<ComboboxItemDto> KeyCombo { get; set; }

        public HelpForEditResponse()
        {
            LanguageCombo = new List<ComboboxItemDto>();
            KeyCombo = new List<ComboboxItemDto>();
        }
    }
}