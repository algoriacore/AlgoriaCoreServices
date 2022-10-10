using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Groups._3Commands;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Emails.Templates._3Commands;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class MailGroupController : BaseController
    {
        #region Mail Group

        [HttpPost]
        public async Task<PagedResultDto<MailGroupListResponse>> GetMailGroupList([FromBody]MailGroupGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_CorreoGrupos_Create)]
        [HttpPost]
        public async Task<long> CreateMailGroup([FromBody]MailGroupCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_CorreoGrupos_Edit)]
        [HttpPost]
        public async Task<long> UpdateMailGroup([FromBody]MailGroupUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<MailGroupForEditResponse> GetMailGroupForEdit([FromBody]MailGroupGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_CorreoGrupos_Copy)]
        [HttpPost]
        public async Task<long> CopyMailGroup([FromBody]MailGroupCopyCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_CorreoGrupos_SetDefault)]
        [HttpPost]
        public async Task<long> CheckMailGroup([FromBody]MailGroupCheckCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_CorreoGrupos_SetDefault)]
        [HttpPost]
        public async Task<long> UnCheckMailGroup([FromBody]MailGroupUnCheckCommand dto)
        {
            return await Mediator.Send(dto);
        }

        #endregion

        #region Mail Template

        [HttpPost]
        public async Task<PagedResultDto<MailTemplateListResponse>> GetMailTemplateList([FromBody]MailTemplateGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_PlantillasCorreo_Create)]
        [HttpPost]
        public async Task<long> CreateMailTemplate([FromBody]MailTemplateCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_PlantillasCorreo_Edit)]
        [HttpPost]
        public async Task<long> UpdateMailTemplate([FromBody]MailTemplateUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<MailTemplateForEditResponse> GetMailTemplateForEdit([FromBody]MailTemplateGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<int> SendTestEmail([FromBody]MailTemplateSendTestCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetMailTemplateGetBodyParamList([FromBody]MailTemplateGetBodyParamListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetMailTemplateGetMailKeyAvailableList([FromBody]MailTemplateGetMailKeyAvailableListQuery query)
        {
            return await Mediator.Send(query);
        }

        #endregion
    }
}