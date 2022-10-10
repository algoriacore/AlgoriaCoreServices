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
    public class CatalogCustomFieldGetComboQueryHandler : BaseCoreClass, IRequestHandler<CatalogCustomFieldGetComboQuery, List<ComboboxItemDto>>
    {
        private readonly CatalogCustomManager _manager;

        public CatalogCustomFieldGetComboQueryHandler(ICoreServices coreServices, CatalogCustomManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<ComboboxItemDto>> Handle(CatalogCustomFieldGetComboQuery request, CancellationToken cancellationToken)
        {
            return await _manager.GetCatalogCustomFieldComboAsync(new CatalogCustomFieldComboFilterDto() { CatalogCustom = request.CatalogCustom });
        }
    }
}
