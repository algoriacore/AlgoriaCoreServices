using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetComboQuery : IRequest<List<ComboboxItemDto>>
    {
        public string Filter { get; set; }
        public string Catalog { get; set; }
        public string FieldName { get; set; }
    }
}
