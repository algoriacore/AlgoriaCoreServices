using AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom;
using AlgoriaCore.Application.QueriesAndCommands.Questionnaires;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplForEditResponse
    {
        public string Id { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public CatalogCustomResponse CatalogCustom { get; set; }
        public QuestionnaireResponse Questionnaire { get; set; }
    }
}