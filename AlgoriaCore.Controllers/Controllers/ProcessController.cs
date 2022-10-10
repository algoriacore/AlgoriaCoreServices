using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Processes;
using AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets;
using AlgoriaCore.Application.QueriesAndCommands.SecurityMembers;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class ProcessController : BaseController
    {
        #region PROCESSES

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes)]
        [HttpPost]
        public async Task<PagedResultDto<Dictionary<string, object>>> GetProcessList([FromBody]ProcessGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetProcessCombo(ProcessGetComboQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ProcessResponse> GetProcess(ProcessGetByIdQuery query)
        {
            return await Mediator.Send(query);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes)]
        [HttpPost]
        public async Task<ProcessForReadResponse> GetProcessForRead(ProcessGetForReadQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes_Create, AppPermissions.Pages_Processes_Processes_Edit)]
        [HttpPost]
        public async Task<ProcessForEditResponse> GetProcessForEdit(ProcessGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes_Create)]
        [HttpPost]
        public async Task<long> CreateProcess([FromBody]ProcessCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes_Edit)]
        [HttpPost]
        public async Task<long> UpdateProcess([FromBody]ProcessUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes_Delete)]
        [HttpPost]
        public async Task<long> DeleteProcess(ProcessDeleteCommand dto)
        {
            return await Mediator.Send(dto);
        }

        #endregion

        #region TIMESHEETS

        [HttpPost("{id}")]
        public async Task<ToDoTimeSheetResponse> GetToDoTimeSheet(long id)
        {
            return await Mediator.Send(new ToDoTimeSheetGetByIdQuery { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes_TimeSheet_Create)]
        [HttpPost]
        public async Task<ToDoTimeSheetForEditResponse> GetToDoTimeSheetForEdit(ToDoTimeSheetGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes_TimeSheet_Create)]
        [HttpPost]
        public async Task<long> CreateToDoTimeSheet([FromBody]ToDoTimeSheetCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes_Edit)]
        [HttpPost]
        public async Task<long> UpdateToDoTimeSheet([FromBody]ToDoTimeSheetUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes_Edit)]
        [HttpPost("{id}")]
        public async Task<long> DeleteToDoTimeSheet(long id)
        {
            return await Mediator.Send(new ToDoTimeSheetDeleteCommand { Id = id });
        }

        #endregion

        #region SECURITY

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes)]
        [HttpPost]
        public async Task<PagedResultDto<ProcessSecurityMemberForListResponse>> GetProcessSecurityMemberList([FromBody] ProcessSecurityMemberGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes_Edit)]
        [HttpPost]
        public async Task<long> CreateProcessSecurityMember([FromBody] ProcessSecurityMemberCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Processes_Processes_Edit)]
        [HttpPost]
        public async Task<long> DeleteProcessSecurityMember(ProcessSecurityMemberDeleteCommand dto)
        {
            return await Mediator.Send(dto);
        }

        #endregion
    }
}
