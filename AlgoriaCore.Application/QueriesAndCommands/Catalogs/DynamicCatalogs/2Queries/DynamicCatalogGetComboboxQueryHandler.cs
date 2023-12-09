using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._2Queries
{
    public class DynamicCatalogGetComboboxQueryHandler : BaseCoreClass, IRequestHandler<DynamicCatalogGetComboboxQuery, List<ComboboxItemDto>>
    {
        private readonly DynamicCatalogsManager _manager;

        public DynamicCatalogGetComboboxQueryHandler(ICoreServices coreServices, DynamicCatalogsManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ComboboxItemDto>> Handle(DynamicCatalogGetComboboxQuery request, CancellationToken cancellationToken)
        {
            return await _manager.GetCatalogosDinamicosListAsync();
        }
    }
}
