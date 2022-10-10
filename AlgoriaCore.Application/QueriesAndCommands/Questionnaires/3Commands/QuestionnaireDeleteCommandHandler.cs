using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Questionnaires;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireDeleteCommandHandler : BaseCoreClass, IRequestHandler<QuestionnaireDeleteCommand, string>
    {
        private readonly QuestionnaireManager _manager;

        public QuestionnaireDeleteCommandHandler(ICoreServices coreServices, QuestionnaireManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<string> Handle(QuestionnaireDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.DeleteQuestionnaireAsync(request.Id);

            return request.Id;
        }
    }
}
