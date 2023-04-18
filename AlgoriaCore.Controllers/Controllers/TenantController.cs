using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands;
using AlgoriaCore.Application.QueriesAndCommands.Tenants;
using AlgoriaCore.Application.QueriesAndCommands.Tenants._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Tenants._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Tenants._3Commands;
using AlgoriaCore.Application.QueriesAndCommands.Users._2Queries;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Domain.MultiTenancy;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter(MultiTenancySide = (byte)MultiTenancySides.Host)]
    public class TenantController : BaseController
    {
        [HttpPost]
        public async Task<FileDto> ExportTenant([FromBody] TenantExportQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportCSVTenant([FromBody] TenantExportCSVQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportPDFTenant([FromBody] TenantExportPDFQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        public async Task<TenantResponse> GetTenantById(int id)
        {
            return await Mediator.Send(new GetTenantQuery { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Tenants_Create)]
        [HttpPost]
        public async Task<int> CreateTenant(TenantCreateCommand command)
        {
            return await Mediator.Send(command);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Tenants_Edit)]
        [HttpPost]
        public async Task<int> UpdateTenant(UpdateTenantCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        public async Task<PagedResultDto<TenantListResponse>> GetTenantsList([FromBody]TenantGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<List<TenantListResponse>> GetTenantsListCompleter([FromBody]TenantGetListCompleterQuery query)
        {
            return await Mediator.Send(query);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Tenants_Delete)]
        [HttpPost]
        public async Task<int> DeleteTenant([FromBody]TenantDeleteCommand dto)
        {
            return await Mediator.Send(dto);
        }
    }
}