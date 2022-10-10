using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireResponse
    {
        public string Id { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string Name { get; set; }
        public string CustomCode { get; set; }
        public string UserCreator { get; set; }
        public bool IsActive { get; set; }

        public List<QuestionnaireSectionResponse> Sections { get; set; }

        public QuestionnaireResponse()
        {
            Sections = new List<QuestionnaireSectionResponse>();
        }
    }
}