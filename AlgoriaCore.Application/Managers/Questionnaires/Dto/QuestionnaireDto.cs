using System;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.Questionnaires.Dto
{
    public class QuestionnaireDto
    {
        public string Id { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string Name { get; set; }
        public string CustomCode { get; set; }
        public string UserCreator { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
        public List<QuestionnaireSectionDto> Sections { get; set; }

        public QuestionnaireDto()
        {
            Sections = new List<QuestionnaireSectionDto>();
        }
    }
}
