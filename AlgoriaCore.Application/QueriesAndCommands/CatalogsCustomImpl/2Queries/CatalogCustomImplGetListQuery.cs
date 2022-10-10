using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Domain.Attributes;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    [MongoTransactional]
    public class CatalogCustomImplGetListQuery : PageListByDto, IRequest<PagedResultDto<Dictionary<string, object>>>
    {
        public string Catalog { get; set; }
    }
}
