using AlgoriaCore.Domain.Attributes;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    [MongoTransactional]
    public class QuestionnaireDeleteCommand : IRequest<string>
    {
        public string Id { get; set; }
    }
}