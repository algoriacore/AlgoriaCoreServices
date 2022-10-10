using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class CatalogCustomController : BaseController
    {
        #region CatalogCustom

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom)]
        [HttpPost]
        public async Task<PagedResultDto<CatalogCustomForListResponse>> GetCatalogCustomList([FromBody] CatalogCustomGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetCatalogCustomCombo(CatalogCustomGetComboQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}")]
        public async Task<CatalogCustomResponse> GetCatalogCustom(string id)
        {
            return await Mediator.Send(new CatalogCustomGetByIdQuery { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom_Create, AppPermissions.Pages_CatalogsCustom_Edit)]
        [HttpPost]
        public async Task<CatalogCustomForEditResponse> GetCatalogCustomForEdit(CatalogCustomGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom_Create, AppPermissions.Pages_CatalogsCustom_Edit)]
        [HttpPost]
        public async Task<string> CreateCatalogCustom([FromBody] CatalogCustomCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom_Edit)]
        [HttpPost]
        public async Task<string> UpdateCatalogCustom([FromBody] CatalogCustomUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_CatalogsCustom_Delete)]
        [HttpPost("{id}")]
        public async Task<string> DeleteCatalogCustom(string id)
        {
            return await Mediator.Send(new CatalogCustomDeleteCommand { Id = id });
        }

        #endregion

        #region CatalogCustom Fields

        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetCatalogCustomFieldCombo(CatalogCustomFieldGetComboQuery query)
        {
            return await Mediator.Send(query);
        }

        #endregion
    }
}
