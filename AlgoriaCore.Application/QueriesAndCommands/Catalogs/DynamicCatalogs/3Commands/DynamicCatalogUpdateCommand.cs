using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._3Commands
{
    public class DynamicCatalogUpdateCommand : IRequest<long>
    {
        public string Tabla { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
