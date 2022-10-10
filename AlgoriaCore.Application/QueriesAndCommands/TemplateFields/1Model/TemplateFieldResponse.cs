using AlgoriaCore.Application.Managers.Templates.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldResponse
    {
        public long Id { get; set; }
        public long? TemplateSection { get; set; }
        public string TemplateSectionDesc { get; set; }
        public string TemplateSectionIconAF { get; set; }
        public short? TemplateSectionOrder { get; set; }
        public string Name { get; set; }
        public string FieldName { get; set; }
        public TemplateFieldType? FieldType { get; set; }
        public short? FieldSize { get; set; }
        public TemplateFieldControl? FieldControl { get; set; }
        public string InputMask { get; set; }
        public bool HasKeyFilter { get; set; }
        public string KeyFilter { get; set; }
        public TemplateFieldStatus? Status { get; set; }
        public bool IsRequired { get; set; }
        public bool ShowOnGrid { get; set; }
        public short? Order { get; set; }
        public bool InheritSecurity { get; set; }

        public long? TemplateFieldRelationTemplate { get; set; }
        public string TemplateFieldRelationTemplateDesc { get; set; }
        public long? TemplateFieldRelationTemplateField { get; set; }
        public List<TemplateFieldOptionDto> Options { get; set; }
        public bool MustHaveOptions { get; set; }
    }
}