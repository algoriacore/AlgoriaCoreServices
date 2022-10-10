using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.Questionnaires.Dto
{
    public class QuestionnaireSectionDto
    {
        public string IconAF { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<QuestionnaireFieldDto> Fields { get; set; }

        public QuestionnaireSectionDto()
        {
            Fields = new List<QuestionnaireFieldDto>();
        }
    }
}
