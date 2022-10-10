using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits
{
    public class OrgUnitGetComboQuery : IRequest<List<ComboboxItemDto>>
    {
        public string Filter { get; set; }
    }
}
