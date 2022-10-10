using AlgoriaCore.Application.Managers.Questionnaires.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireFieldResponse
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
        public List<QuestionnaireFieldOptionResponse> Options { get; set; }
        public QuestionnaireCatalogCustomResponse CatalogCustom { get; set; }
        public QuestionnaireCustomPropertiesResponse CustomProperties { get; set; }
        public bool MustHaveOptions { get; set; }

        public QuestionnaireFieldResponse()
        {
            Options = new List<QuestionnaireFieldOptionResponse>();
        }
    }
}