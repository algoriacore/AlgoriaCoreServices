using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireGetComboQuery : IRequest<List<ComboboxItemDto>>
    {
        public string Filter { get; set; }
        public bool? IsActive { get; set; }
    }
}
