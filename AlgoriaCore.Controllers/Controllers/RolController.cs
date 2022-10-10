using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class RolController : BaseController
    {
        [HttpPost]
        public async Task<PagedResultDto<RolForListResponse>> GetRolList([FromBody]RolGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<RolResponse> Get([FromBody]RolGetByIdQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Roles_Create)]
        [HttpPost]
        public async Task<long> CreateRol([FromBody]RolCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Roles_Edit)]
        [HttpPost]
        public async Task<long> UpdateRol([FromBody]RolUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Roles_Delete)]
        [HttpPost]
        public async Task<long> DeleteRol([FromBody]RolDeleteCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<RolForEditReponse> GetRolForEdit([FromBody]RolGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<List<RolForListActiveResponse>> GetRolListActive([FromBody]RolGetForListActiveQuery query)
        {
            return await Mediator.Send(query);
        }

    }
}
