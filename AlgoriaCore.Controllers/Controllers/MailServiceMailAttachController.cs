using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Controllers;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailAttachs;

namespace AlgoriaCore.WebAPI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class MailServiceMailAttachController : BaseController
    {


        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_MailServiceMailAttach)]
        [HttpPost]
        public async Task<PagedResultDto<MailServiceMailAttachListResponse>> GetMailServiceMailAttachPagedListAsync([FromBody] MailServiceMailAttachGetListQuery query)
        {
            return await Mediator.Send(query);
        }


        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_MailServiceMailAttach)]
        [HttpPost]
        public async Task<MailServiceMailAttachForEditResponse> GetMailServiceMailAttachForEditAsync(MailServiceMailAttachGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_MailServiceMailAttach)]
        [HttpPost]
        public async Task<MailServiceMailAttachForEditResponse> GetMailServiceMailAttachFileAsync(MailServiceMailAttachGetFileQuery dto)
        {
            return await Mediator.Send(dto);
        }

    }
}

