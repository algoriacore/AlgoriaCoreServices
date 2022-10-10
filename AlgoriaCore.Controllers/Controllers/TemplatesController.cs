using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.TemplateFields;
using AlgoriaCore.Application.QueriesAndCommands.Templates;
using AlgoriaCore.Application.QueriesAndCommands.TemplateSections;
using AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers;
using AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [Authorize]
    public class TemplatesController : BaseController
    {
        #region TEMPLATES

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates)]
        [HttpPost]
        public async Task<List<TemplateForListResponse>> GetTemplateNoPagedList()
        {
            return await Mediator.Send(new TemplateGetNoPagedListQuery());
        }

        [HttpPost("{id}")]
        public async Task<TemplateResponse> GetTemplate(long id)
        {
            return await Mediator.Send(new TemplateGetByIdQuery { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<TemplateForEditResponse> GetTemplateForEdit(TemplateGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Create)]
        [HttpPost]
        public async Task<long> CreateTemplate([FromBody]TemplateCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<long> UpdateTemplate([FromBody]TemplateUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Delete)]
        [HttpPost("{id}")]
        public async Task<long> DeleteTemplate(long id)
        {
            return await Mediator.Send(new TemplateDeleteCommand { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Create, AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<long> GenerateDbTable(long id)
        {
            return await Mediator.Send(new TemplateGenerateDbTableCommand { Id = id });
        }

        #endregion

        #region TEMPLATES SECTIONS

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates)]
        [HttpPost]
        public async Task<PagedResultDto<TemplateSectionForListResponse>> GetTemplateSectionList([FromBody]TemplateSectionGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}")]
        public async Task<TemplateSectionResponse> GetTemplateSection(long id)
        {
            return await Mediator.Send(new TemplateSectionGetByIdQuery { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<TemplateSectionForEditResponse> GetTemplateSectionForEdit(TemplateSectionGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<long> CreateTemplateSection([FromBody]TemplateSectionCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<long> UpdateTemplateSection([FromBody]TemplateSectionUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost("{id}")]
        public async Task<long> DeleteTemplateSection(long id)
        {
            return await Mediator.Send(new TemplateSectionDeleteCommand { Id = id });
        }

        #endregion

        #region TEMPLATES FIELDS

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates)]
        [HttpPost]
        public async Task<PagedResultDto<TemplateFieldForListResponse>> GetTemplateFieldList([FromBody]TemplateFieldGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<List<TemplateFieldResponse>> GetTemplateFieldListByTemplate(TemplateFieldGetListByTemplateQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetTemplateFieldCombo(TemplateFieldGetComboQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}")]
        public async Task<TemplateFieldResponse> GetTemplateField(long id)
        {
            return await Mediator.Send(new TemplateFieldGetByIdQuery { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<TemplateFieldForEditResponse> GetTemplateFieldForEdit(TemplateFieldGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<long> CreateTemplateField([FromBody]TemplateFieldCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<long> UpdateTemplateField([FromBody]TemplateFieldUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost("{id}")]
        public async Task<long> DeleteTemplateField(long id)
        {
            return await Mediator.Send(new TemplateFieldDeleteCommand { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<short> GetTemplateFieldNextOrderByTemplateSection(long templateSection)
        {
            return await Mediator.Send(new TemplateFieldNextOrderGetByTemplateSectionQuery { TemplateSection = templateSection });
        }

        #endregion

        #region ACTIVITY STATUS

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates)]
        [HttpPost]
        public async Task<PagedResultDto<TemplateToDoStatusForListResponse>> GetTemplateToDoStatusList([FromBody]TemplateToDoStatusGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}")]
        public async Task<TemplateToDoStatusResponse> GetTemplateToDoStatus(long id)
        {
            return await Mediator.Send(new TemplateToDoStatusGetByIdQuery { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<TemplateToDoStatusForEditResponse> GetTemplateToDoStatusForEdit(TemplateToDoStatusGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<long> CreateTemplateToDoStatus([FromBody]TemplateToDoStatusCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<long> UpdateTemplateToDoStatus([FromBody]TemplateToDoStatusUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost("{id}")]
        public async Task<long> DeleteTemplateToDoStatus(long id)
        {
            return await Mediator.Send(new TemplateToDoStatusDeleteCommand { Id = id });
        }

        #endregion

        #region SECURITY

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates)]
        [HttpPost]
        public async Task<PagedResultDto<TemplateSecurityMemberForListResponse>> GetTemplateSecurityMemberList([FromBody] TemplateSecurityMemberGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<long> CreateTemplateSecurityMember([FromBody] TemplateSecurityMemberCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Templates_Edit)]
        [HttpPost]
        public async Task<long> DeleteTemplateSecurityMember(TemplateSecurityMemberDeleteCommand dto)
        {
            return await Mediator.Send(dto);
        }

        #endregion
    }
}
