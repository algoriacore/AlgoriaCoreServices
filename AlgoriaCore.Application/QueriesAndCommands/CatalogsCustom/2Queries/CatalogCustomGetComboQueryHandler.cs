using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.CatalogsCustom.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomGetComboQueryHandler : BaseCoreClass, IRequestHandler<CatalogCustomGetComboQuery, List<ComboboxItemDto>>
    {
        private readonly CatalogCustomManager _manager;

        public CatalogCustomGetComboQueryHandler(ICoreServices coreServices, CatalogCustomManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ComboboxItemDto>> Handle(CatalogCustomGetComboQuery request, CancellationToken cancellationToken)
        {
            return await _manager.GetCatalogCustomComboAsync(new CatalogCustomComboFilterDto() { Filter = request.Filter, IsActive = request.IsActive });
        }
    }
}
