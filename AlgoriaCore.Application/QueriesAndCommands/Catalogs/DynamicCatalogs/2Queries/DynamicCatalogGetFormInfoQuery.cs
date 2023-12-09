using AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._2Queries
{
    public class DynamicCatalogGetFormInfoQuery: IRequest<DynamicCatalogFormInfoResponse>
    {
        public string Tabla { get; set; }
    }
}
