using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.Templates.Dto
{
    public class TemplateFieldDto
    {
        public long? Id { get; set; }
        public long? TemplateSection { get; set; }
        public string TemplateSectionDesc { get; set; }
        public string TemplateSectionIconAF { get; set; }
        public short? TemplateSectionOrder { get; set; }
        public long? Template { get; set; }
        public string TemplateDesc { get; set; }
        public string TemplateTableName { get; set; }
        public string Name { get; set; }
        public string FieldName { get; set; }
        public TemplateFieldType? FieldType { get; set; }
        public string FieldTypeDesc { get; set; }
        public short? FieldSize { get; set; }
        public TemplateFieldControl? FieldControl { get; set; }
        public string FieldControlDesc { get; set; }
        public string InputMask { get; set; }
        public bool HasKeyFilter { get; set; }
        public string HasKeyFilterDesc { get; set; }
        public string KeyFilter { get; set; }
        public TemplateFieldStatus? Status { get; set; }
        public string StatusDesc { get; set; }
        public bool IsRequired { get; set; }
        public string IsRequiredDesc { get; set; }
        public bool ShowOnGrid { get; set; }
        public string ShowOnGridDesc { get; set; }
        public short? Order { get; set; }
        public bool InheritSecurity { get; set; }
        public string InheritSecurityDesc { get; set; }

        public long? TemplateFieldRelationTemplate { get; set; }
        public string TemplateFieldRelationTemplateDesc { get; set; }
        public string TemplateFieldRelationTemplateTableName { get; set; }
        public long? TemplateFieldRelationTemplateField { get; set; }
        public string TemplateFieldRelationTemplateFieldDesc { get; set; }
        public string TemplateFieldRelationTemplateFieldName { get; set; }

        public List<TemplateFieldOptionDto> Options { get; set; }
        public bool MustHaveOptions { get; set; }
        
        public TemplateFieldDto() {
            Options = new List<TemplateFieldOptionDto>();
        }
    }

    public enum TemplateFieldType: short
    {
        Boolean = 1,
        Text = 10,
        Multivalue = 20,
        Date = 30,
        DateTime = 31,
        Time = 32,
        Integer = 40,
        Decimal = 41,
        GoogleAddress = 50,
        Template = 60,
        User = 70
    }

    public enum TemplateFieldControl : short
    {
        // TEXT
        InputText = 10,
        DropDown = 11,
        Listbox = 12,
        RadioButton = 13,
        InputSwitch = 14,
        InputMask = 15,
        InputTextArea = 16,
        // MULTIVALUE
        ListboxMultivalue = 21,
        Checkbox = 22,
        Multiselect = 23,
        // DATE AND TIME
        CalendarBasic = 30, // date
        CalendarTime = 31, // date and time
        CalendarTimeOnly = 32, // time
        // NUMBERS
        Spinner = 40, // integer
        SpinnerFormatInput = 41, // decimal
        TextNumber = 42, // integer/decimal
        // GOOGLE ADDRESS
        GoogleAddress = 50,
        // TEMPLATE/USERS
        Autocomplete = 60,
        AutocompleteDynamic = 61
    }

    public enum TemplateFieldStatus : byte
    {
        New = 1,
        Processed = 2,
        Modified = 3,
        Deleted = 4
    }
}
