using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireSectionResponse
    {
        public string IconAF { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<QuestionnaireFieldResponse> Fields { get; set; }

        public QuestionnaireSectionResponse()
        {
            Fields = new List<QuestionnaireFieldResponse>();
        }
    }
}