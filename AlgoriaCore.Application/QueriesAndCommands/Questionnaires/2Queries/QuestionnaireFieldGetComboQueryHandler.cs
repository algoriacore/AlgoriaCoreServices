using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Questionnaires;
using AlgoriaCore.Application.Managers.Questionnaires.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireFieldGetComboQueryHandler : BaseCoreClass, IRequestHandler<QuestionnaireFieldGetComboQuery, List<ComboboxItemDto>>
    {
        private readonly QuestionnaireManager _manager;

        public QuestionnaireFieldGetComboQueryHandler(ICoreServices coreServices, QuestionnaireManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ComboboxItemDto>> Handle(QuestionnaireFieldGetComboQuery request, CancellationToken cancellationToken)
        {
            return await _manager.GetQuestionnaireFieldComboAsync(new QuestionnaireFieldComboFilterDto() { Questionnaire = request.Questionnaire });
        }
    }
}
