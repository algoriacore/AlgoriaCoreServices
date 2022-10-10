using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Templates.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldForEditResponse
    {
        public long? Id { get; set; }
        public long? TemplateSection { get; set; }
        public string TemplateSectionDesc { get; set; }
        public string TemplateSectionIconAF { get; set; }
        public string Name { get; set; }
        public string FieldName { get; set; }
        public TemplateFieldType? FieldType { get; set; }
        public short? FieldSize { get; set; }
        public TemplateFieldControl? FieldControl { get; set; }
        public string InputMask { get; set; }
        public bool HasKeyFilter { get; set; }
        public string KeyFilter { get; set; }
        public TemplateFieldStatus? Status { get; set; }
        public string StatusDesc { get; set; }
        public bool IsRequired { get; set; }
        public bool ShowOnGrid { get; set; }
        public short? Order { get; set; }
        public bool InheritSecurity { get; set; }

        public long? TemplateFieldRelationTemplate { get; set; }
        public string TemplateFieldRelationTemplateDesc { get; set; }
        public long? TemplateFieldRelationTemplateField { get; set; }
        public string TemplateFieldRelationTemplateFieldDesc { get; set; }

        public List<TemplateFieldOptionDto> Options { get; set; }

        public List<ComboboxItemDto> TemplateCombo { get; set; }
        public List<ComboboxItemDto> TemplateFieldCombo { get; set; }

        public TemplateFieldForEditResponse() {
            Options = new List<TemplateFieldOptionDto>();
            TemplateFieldCombo = new List<ComboboxItemDto>();
        }
    }
}