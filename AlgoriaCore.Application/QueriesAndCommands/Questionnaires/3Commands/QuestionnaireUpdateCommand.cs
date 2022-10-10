using AlgoriaCore.Domain.Attributes;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    [MongoTransactional]
    public class QuestionnaireUpdateCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string CustomCode { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public List<QuestionnaireSectionResponse> Sections { get; set; }

        public QuestionnaireUpdateCommand()
        {
            Sections = new List<QuestionnaireSectionResponse>();
        }
    }
}