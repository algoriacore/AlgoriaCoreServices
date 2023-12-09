using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._2Queries
{
    public class DataListQuery : IRequest<List<Dictionary<string, object>>>
    {
        public string Tabla { get; set; }

        public string Sorting { get; set; }
        public string Filter { get; set; }


        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }

        public bool IsPaged { get; set; }
    }
}
