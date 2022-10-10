using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireFieldGetComboQuery : IRequest<List<ComboboxItemDto>>
    {
        public string Questionnaire { get; set; }
    }
}
