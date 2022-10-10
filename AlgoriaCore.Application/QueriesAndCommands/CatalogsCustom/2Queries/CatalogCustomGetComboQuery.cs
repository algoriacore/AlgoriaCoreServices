using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomGetComboQuery : IRequest<List<ComboboxItemDto>>
    {
        public string Filter { get; set; }
        public bool? IsActive { get; set; }
    }
}
