using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Questionnaires;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class QuestionnaireController : BaseController
    {
        #region Questionnaire

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Questionnaires)]
        [HttpPost]
        public async Task<PagedResultDto<QuestionnaireForListResponse>> GetQuestionnaireList([FromBody]QuestionnaireGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetQuestionnaireCombo(QuestionnaireGetComboQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}")]
        public async Task<QuestionnaireResponse> GetQuestionnaire(string id)
        {
            return await Mediator.Send(new QuestionnaireGetByIdQuery { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Questionnaires_Create, AppPermissions.Pages_Questionnaires_Edit)]
        [HttpPost]
        public async Task<string> CreateQuestionnaire([FromBody]QuestionnaireCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Questionnaires_Edit)]
        [HttpPost]
        public async Task<string> UpdateQuestionnaire([FromBody]QuestionnaireUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Questionnaires_Delete)]
        [HttpPost("{id}")]
        public async Task<string> DeleteQuestionnaire(string id)
        {
            return await Mediator.Send(new QuestionnaireDeleteCommand { Id = id });
        }

        #endregion

        #region Questionnaire Fields

        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetQuestionnaireFieldCombo(QuestionnaireFieldGetComboQuery query)
        {
            return await Mediator.Send(query);
        }

        #endregion
    }
}
