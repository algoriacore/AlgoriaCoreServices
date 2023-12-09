using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._2Queries
{
    public class DataByIdQuery : IRequest<Dictionary<string, object>>
    {
        public string Tabla { get; set; }
        public string Id { get; set; }
    }
}
