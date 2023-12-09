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
    public class DynamicCatalogGetFormInfoQueryHandler : BaseCoreClass, IRequestHandler<DynamicCatalogGetFormInfoQuery, DynamicCatalogFormInfoResponse>
    {
        private readonly DynamicCatalogsManager _manager;

        public DynamicCatalogGetFormInfoQueryHandler(ICoreServices coreServices, DynamicCatalogsManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<DynamicCatalogFormInfoResponse> Handle(DynamicCatalogGetFormInfoQuery request, CancellationToken cancellationToken)
        {
            DynamicCatalogFormInfoResponse resp = new DynamicCatalogFormInfoResponse();

            var ll = await _manager.GetCatalogoDinamicoFormDefinicionesList(request.Tabla);
            resp.CampoLlavePrimaria = ll.CampoLlavePrimaria;
            resp.DefinitionsList = ll.DefinitionsList.Select(m => new DynamicCatalogDefinitionResponse
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

            resp.ValidationsList = ll.ValidationsList.Select(m => new DynamicCatalogValidationResponse
            {
                Id = m.Id,
                Campo = m.Campo,
                Regla = m.Regla,
                ValorReferencia = m.ValorReferencia
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
