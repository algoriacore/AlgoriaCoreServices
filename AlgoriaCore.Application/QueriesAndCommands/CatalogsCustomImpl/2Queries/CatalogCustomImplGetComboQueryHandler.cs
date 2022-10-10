using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetComboQueryHandler : BaseCoreClass, IRequestHandler<CatalogCustomImplGetComboQuery, List<ComboboxItemDto>>
    {
        private readonly CatalogCustomImplManager _manager;

        public CatalogCustomImplGetComboQueryHandler(ICoreServices coreServices, CatalogCustomImplManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ComboboxItemDto>> Handle(CatalogCustomImplGetComboQuery request, CancellationToken cancellationToken)
        {
            return await _manager.GetCatalogCustomImplComboAsync(new CatalogCustomImplComboFilterDto()
            {
                Catalog = request.Catalog,
                FieldName = request.FieldName,
                Filter = request.Filter
            });
        }
    }
}
