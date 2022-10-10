using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.TemplateFields;
using AlgoriaCore.Application.QueriesAndCommands.Templates;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessForEditResponse
    {
        public TemplateResponse Template { get; set; }
        public List<TemplateFieldResponse> TemplateFields { get; set; }
        public long? Id { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public ProcessToDoActivityForEditResponse Activity { get; set; }

        public List<ComboboxItemDto> ActivityStatusCombo { get; set; }

        public ProcessForEditResponse()
        {
            ActivityStatusCombo = new List<ComboboxItemDto>();
            TemplateFields = new List<TemplateFieldResponse>();
        }
    }
}