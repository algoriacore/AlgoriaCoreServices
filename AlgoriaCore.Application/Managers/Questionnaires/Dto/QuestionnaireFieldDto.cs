using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.Questionnaires.Dto
{
    public class QuestionnaireFieldDto
    {
        public QuestionnaireFieldControl FieldControl { get; set; }
        public string FieldControlDesc { get; set; }
        public string FieldName { get; set; }
        public int? FieldSize { get; set; }
        public QuestionnaireFieldType FieldType { get; set; }
        public string FieldTypeDesc { get; set; }
        public bool HasKeyFilter { get; set; }
        public string InputMask { get; set; }
        public bool IsRequired { get; set; }
        public string IsRequiredDesc { get; set; }
        public string KeyFilter { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<QuestionnaireFieldOptionDto> Options { get; set; }
        public QuestionnaireCatalogCustomDto CatalogCustom { get; set; }
        public bool MustHaveOptions { get; set; }
        public QuestionnaireCustomPropertiesDto CustomProperties { get; set; }

        public QuestionnaireFieldDto()
        {
            Options = new List<QuestionnaireFieldOptionDto>();
        }
    }

    public enum QuestionnaireFieldType : short
    {
        Boolean = 1,
        Text = 10,
        Multivalue = 20,
        Date = 30,
        DateTime = 31,
        Time = 32,
        Integer = 40,
        Decimal = 41,
        Currency = 43,
        GoogleAddress = 50,
        CatalogCustom = 60,
        User = 70
    }

    public enum QuestionnaireFieldControl : short
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
        // TextNumber = 42, // integer/decimal
        TextNumber = 45, // integer/decimal
        // GOOGLE ADDRESS
        GoogleAddress = 50,
        // TEMPLATE/USERS
        Autocomplete = 60,
        AutocompleteDynamic = 61
    }
}
