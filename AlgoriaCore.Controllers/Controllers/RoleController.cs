using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Roles;
using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands;
using AlgoriaCore.Application.QueriesAndCommands.Users._2Queries;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class RoleController : BaseController
    {
        [HttpPost]
        public async Task<PagedResultDto<RoleForListResponse>> GetRoleList([FromBody]RoleGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportRole([FromBody] RoleExportQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportCSVRole([FromBody] RoleExportCSVQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportPDFRole([FromBody] RoleExportPDFQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<RoleResponse> Get([FromBody]RoleGetByIdQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Roles_Create)]
        [HttpPost]
        public async Task<long> CreateRole([FromBody]RoleCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Roles_Edit)]
        [HttpPost]
        public async Task<long> UpdateRole([FromBody]RoleUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Roles_Delete)]
        [HttpPost]
        public async Task<long> DeleteRole([FromBody]RoleDeleteCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<RoleForEditReponse> GetRoleForEdit([FromBody]RoleGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<List<RoleForListActiveResponse>> GetRoleListActive([FromBody]RoleGetForListActiveQuery query)
        {
            return await Mediator.Send(query);
        }

    }
}
