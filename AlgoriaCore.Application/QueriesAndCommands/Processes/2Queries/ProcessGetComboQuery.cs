using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetComboQuery : IRequest<List<ComboboxItemDto>>
    {
        public string Filter { get; set; }
        public long TemplateField { get; set; }
    }
}
