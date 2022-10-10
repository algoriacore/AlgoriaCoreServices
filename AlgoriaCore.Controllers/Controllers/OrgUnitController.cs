using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.OrgUnits.OrgUnits;
using AlgoriaCore.Application.QueriesAndCommands.OrgUnitUsers.OrgUnitUsers;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class OrgUnitController : BaseController
    {
        #region Organization Units

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_OrgUnits)]
        [HttpPost]
        public async Task<PagedResultDto<OrgUnitForListResponse>> GetOrgUnitList([FromBody]OrgUnitGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_OrgUnits)]
        [HttpPost]
        public async Task<List<OrgUnitForListResponse>> GetOrgUnitByParentOUList(long parentOU)
        {
            return await Mediator.Send(new OrgUnitGetByParentOUListQuery() { ParentOU = parentOU });
        }

        [HttpPost("{id}")]
        public async Task<OrgUnitResponse> GetOrgUnit(long id)
        {
            return await Mediator.Send(new OrgUnitGetByIdQuery { Id = id });
        }

        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetOrgUnitCombo(OrgUnitGetComboQuery query)
        {
            return await Mediator.Send(query);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_OrgUnits_Create, AppPermissions.Pages_Administration_OrgUnits_Edit)]
        [HttpPost]
        public async Task<OrgUnitForEditResponse> GetOrgUnitForEdit(OrgUnitGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_OrgUnits_Create, AppPermissions.Pages_Administration_OrgUnits_Edit)]
        [HttpPost]
        public async Task<long> CreateOrgUnit([FromBody]OrgUnitCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_OrgUnits_Edit)]
        [HttpPost]
        public async Task<long> UpdateOrgUnit([FromBody]OrgUnitUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_OrgUnits_Delete)]
        [HttpPost("{id}")]
        public async Task<long> DeleteOrgUnit(long id)
        {
            return await Mediator.Send(new OrgUnitDeleteCommand { Id = id });
        }

        #endregion

        #region Users

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_OrgUnits)]
        [HttpPost]
        public async Task<PagedResultDto<OrgUnitUserForListResponse>> GetOrgUnitUserList([FromBody]OrgUnitUserGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_OrgUnits_Create, AppPermissions.Pages_Administration_OrgUnits_Edit)]
        [HttpPost]
        public async Task<long> CreateOrgUnitUser([FromBody]OrgUnitUserCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_OrgUnits_Create, AppPermissions.Pages_Administration_OrgUnits_Edit)]
        [HttpPost("{id}")]
        public async Task<long> DeleteOrgUnitUser(long id)
        {
            return await Mediator.Send(new OrgUnitUserDeleteCommand { Id = id });
        }

        #endregion
    }
}
