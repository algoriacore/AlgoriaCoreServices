using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._3Commands
{
    public class DynamicCatalogDeleteCommand : IRequest<long>
    {
        public string Tabla { get; set; }
        public string Id { get; set; }
    }
}
