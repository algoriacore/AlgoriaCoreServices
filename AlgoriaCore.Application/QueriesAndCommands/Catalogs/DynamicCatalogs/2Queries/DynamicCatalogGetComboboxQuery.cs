using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._2Queries
{
    public class DynamicCatalogGetComboboxQuery: IRequest<List<ComboboxItemDto>>
    {
    }
}
