using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos;
using AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._1Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._2Queries
{
    public class DynamicCatalogGetVistaInfoQueryHandler : BaseCoreClass, IRequestHandler<DynamicCatalogGetVistaInfoQuery, DynamicCatalogViewInfoResponse>
    {
        private readonly DynamicCatalogsManager _manager;

        public DynamicCatalogGetVistaInfoQueryHandler(ICoreServices coreServices, DynamicCatalogsManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<DynamicCatalogViewInfoResponse> Handle(DynamicCatalogGetVistaInfoQuery request, CancellationToken cancellationToken)
        {
            DynamicCatalogViewInfoResponse resp = new DynamicCatalogViewInfoResponse();

            var ll = await _manager.GetCatalogoDinamicoVistaDefinicionesList(request.Tabla);

            resp.CampoLlavePrimaria = ll.CampoLlavePrimaria;
            resp.CamposVistaList = ll.CamposVistaList;
            resp.DefinitionsList = ll.DefinicionesList.Select(m => new DynamicCatalogDefinitionResponse
            {
                Id = m.Id,
                Campo = m.Campo,
                Tipo = m.Tipo,
                Longitud = m.Longitud,
                Decimales = m.Decimales,
                MostrarEnVista = m.MostrarEnVista,
                CapturarEnPantalla = m.CapturarEnPantalla,
                Posicion = m.Posicion
            }).ToList();

            resp.RelationsList = ll.RelationsList.Select(m => new DynamicCatalogRelationResponse
            {
                Id = m.Id,
                CampoRelacion = m.CampoRelacion,
                TablaRelacionada = m.TablaRelacionada,
                CampoReferenciado = m.CampoReferenciado,
                CampoDescReferenciado = m.CampoDescReferenciado
            }).ToList();

            return resp;
        }
    }
}
