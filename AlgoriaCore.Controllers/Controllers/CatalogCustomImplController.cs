using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class CatalogCustomImplController : BaseController
    {
        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom_CatalogsCustom)]
        [HttpPost]
        public async Task<PagedResultDto<Dictionary<string, object>>> GetCatalogCustomImplList([FromBody] CatalogCustomImplGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetCatalogCustomImplCombo(CatalogCustomImplGetComboQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<CatalogCustomImplResponse> GetProcess(CatalogCustomImplGetByIdQuery query)
        {
            return await Mediator.Send(query);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom_CatalogsCustom)]
        [HttpPost]
        public async Task<CatalogCustomImplForReadResponse> GetCatalogCustomImplForRead(CatalogCustomImplGetForReadQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom_CatalogsCustom_Create, AppPermissions.Pages_CatalogsCustom_CatalogsCustom_Edit)]
        [HttpPost]
        public async Task<CatalogCustomImplForEditResponse> GetCatalogCustomImplForEdit(CatalogCustomImplGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom_CatalogsCustom_Create)]
        [HttpPost]
        public async Task<string> CreateCatalogCustomImpl([FromBody] CatalogCustomImplCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom_CatalogsCustom_Edit)]
        [HttpPost]
        public async Task<string> UpdateCatalogCustomImpl([FromBody] CatalogCustomImplUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom_CatalogsCustom_Delete)]
        [HttpPost]
        public async Task<string> DeleteCatalogCustomImpl(CatalogCustomImplDeleteCommand dto)
        {
            return await Mediator.Send(dto);
        }
    }
}
