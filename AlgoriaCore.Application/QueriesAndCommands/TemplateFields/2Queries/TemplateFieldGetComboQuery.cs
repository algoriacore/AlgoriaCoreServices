using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldGetComboQuery : IRequest<List<ComboboxItemDto>>
    {
        public long? Template { get; set; }
    }
}
