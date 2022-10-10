using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Languages.Languages;
using AlgoriaCore.Application.QueriesAndCommands.Languages.Texts;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class LanguageController : BaseController
    {
        #region Lenguajes

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Languages)]
        [HttpPost]
        public async Task<PagedResultDto<LanguageForListResponse>> GetLanguageList([FromBody]LanguageGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}")]
        public async Task<LanguageResponse> GetLanguage(int id)
        {
            return await Mediator.Send(new LanguageGetByIdQuery { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Languages_Edit)]
        [HttpPost]
        public async Task<LanguageForEditResponse> GetLanguageForEdit(LanguageGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Languages_Create, AppPermissions.Pages_Administration_Languages_Edit)]
        [HttpPost]
        public async Task<int> CreateLanguage([FromBody]LanguageCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Languages_Edit)]
        [HttpPost]
        public async Task<int> UpdateLanguage([FromBody]LanguageUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Languages_Delete)]
        [HttpPost("{id}")]
        public async Task<int> DeleteLanguage(int id)
        {
            return await Mediator.Send(new LanguageDeleteCommand { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Languages_Edit)]
        public async Task<int> SetLanguageDefault(int language)
        {
            return await Mediator.Send(new LanguageSetDefaultCommand { Language = language });
        }

        #endregion

        #region Textos

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Languages_ChangeTexts)]
        [HttpPost]
        public async Task<PagedResultDto<LanguageTextForListResponse>> GetLanguageTextList([FromBody]LanguageTextGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}")]
        public async Task<LanguageTextResponse> GetLanguageText(int id)
        {
            return await Mediator.Send(new LanguageTextGetByIdQuery { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Languages_ChangeTexts)]
        [HttpPost]
        public async Task<LanguageTextForEditResponse> GetLanguageTextForEdit(LanguageTextGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Languages_ChangeTexts)]
        [HttpPost]
        public async Task<long> UpdateLanguageText([FromBody]LanguageTextUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        #endregion
    }
}
