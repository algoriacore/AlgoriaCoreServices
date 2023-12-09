using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._2Queries
{
    public class DataListQueryHandler : BaseCoreClass, IRequestHandler<DataListQuery, List<Dictionary<string, object>>>
    {
        private readonly DynamicCatalogsManager _manager;

        public DataListQueryHandler(ICoreServices coreServices, DynamicCatalogsManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<Dictionary<string, object>>> Handle(DataListQuery request, CancellationToken cancellationToken)
        {
            var ll = await _manager.GetList(new Managers.Catalogos.CatalogosDinamicos.Dto.DynamicCatalogListFilterDto
            {
                Tabla = request.Tabla,
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            });

            return ll;
        }
    }
}
