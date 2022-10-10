using AlgoriaCore.Application.Managers.Templates.Dto;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldForListResponse
    {
        public long Id { get; set; }
        public long? TemplateSection { get; set; }
        public string TemplateSectionDesc { get; set; }
        public string TemplateSectionIconAF { get; set; }
        public short? TemplateSectionOrder { get; set; }
        public string Name { get; set; }
        public string FieldName { get; set; }
        public string FieldTypeDesc { get; set; }
        public short? FieldSize { get; set; }
        public string FieldControlDesc { get; set; }
        public string InputMask { get; set; }
        public string KeyFilter { get; set; }
        public TemplateFieldStatus Status { get; set; }
        public string StatusDesc { get; set; }
        public string IsRequiredDesc { get; set; }
        public string ShowOnGridDesc { get; set; }
        public short? Order { get; set; }
        public bool MustHaveOptions { get; set; }
    }
}