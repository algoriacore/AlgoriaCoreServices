using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Controllers;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailConfigs;

namespace AlgoriaCore.WebAPI.Controllers
{
   [AlgoriaCoreAuthorizationFilter]
   public class MailServiceMailConfigController : BaseController
   {

       [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_MailServiceMailConfig)]
       [HttpPost]
       public async Task<MailServiceMailConfigForEditResponse> GetMailServiceMailConfigForEditAsync(MailServiceMailConfigGetForEditQuery dto)
       {
           return await Mediator.Send(dto);
       }

   }
}

