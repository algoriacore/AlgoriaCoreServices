using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireGetByIdQuery : IRequest<QuestionnaireResponse>
    {
        public string Id { get; set; }
    }
}
