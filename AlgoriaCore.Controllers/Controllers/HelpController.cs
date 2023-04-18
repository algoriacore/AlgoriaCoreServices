using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Helps;
using AlgoriaCore.Application.QueriesAndCommands.Users._2Queries;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class HelpController : BaseController
    {
        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Helps)]
        [HttpPost]
        public async Task<PagedResultDto<HelpForListResponse>> GetHelpList([FromBody]HelpGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportHelp([FromBody] HelpExportQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportCSVHelp([FromBody] HelpExportCSVQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportPDFHelp([FromBody] HelpExportPDFQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}")]
        public async Task<HelpResponse> GetHelp(long id)
        {
            return await Mediator.Send(new HelpGetByIdQuery { Id = id });
        }

        [HttpPost("{key}")]
        public async Task<HelpResponse> GetHelpByKeyForCurrentUser(string key)
        {
            return await Mediator.Send(new HelpGetByKeyForCurrentUserQuery { Key = key });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Helps_Create, AppPermissions.Pages_Administration_Helps_Edit)]
        [HttpPost]
        public async Task<HelpForEditResponse> GetHelpForEdit(HelpGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Helps_Create, AppPermissions.Pages_Administration_Helps_Edit)]
        [HttpPost]
        public async Task<long> CreateHelp([FromBody]HelpCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Helps_Edit)]
        [HttpPost]
        public async Task<long> UpdateHelp([FromBody]HelpUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Helps_Delete)]
        [HttpPost("{id}")]
        public async Task<int> DeleteHelp(int id)
        {
            return await Mediator.Send(new HelpDeleteCommand { Id = id });
        }
    }
}
