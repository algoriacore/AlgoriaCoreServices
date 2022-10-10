using AlgoriaCore.Domain.Attributes;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    [MongoTransactional]
    public class QuestionnaireCreateCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string CustomCode { get; set; }
        public bool IsActive { get; set; }

        public List<QuestionnaireSectionResponse> Sections { get; set; }

        public QuestionnaireCreateCommand()
        {
            Sections = new List<QuestionnaireSectionResponse>();
        }
    }
}