using AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._2Queries
{
    public class DynamicCatalogGetVistaInfoQuery: IRequest<DynamicCatalogViewInfoResponse>
    {
        public string Tabla { get; set; }
    }
}
