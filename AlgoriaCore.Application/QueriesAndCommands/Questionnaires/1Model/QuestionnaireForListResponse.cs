using System;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireForListResponse
    {
        public string Id { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string Name { get; set; }
        public string UserCreator { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveDesc { get; set; }
    }
}