using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMails;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Controllers;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebAPI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
   public class MailServiceMailController : BaseController
   {

       [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_MailServiceMail)]
       [HttpPost]
       public async Task<PagedResultDto<MailServiceMailListResponse>> GetMailServiceMailPagedListAsync([FromBody]MailServiceMailGetListQuery query)
       {
           return await Mediator.Send(query);
       }

       [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_MailServiceMail)]
       [HttpPost]
       public async Task<MailServiceMailForEditResponse> GetMailServiceMailForEditAsync(MailServiceMailGetForEditQuery dto)
       {
           return await Mediator.Send(dto);
       }

   }
}

