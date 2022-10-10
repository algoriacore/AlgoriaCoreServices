using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomFieldGetComboQuery : IRequest<List<ComboboxItemDto>>
    {
        public string CatalogCustom { get; set; }
    }
}
