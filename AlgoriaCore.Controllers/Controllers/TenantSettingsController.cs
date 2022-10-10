using AlgoriaCore.Application.QueriesAndCommands.Settings.Tenant;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Tenant_Settings)]
    public class TenantSettingsController : BaseController
    {
        [HttpPost]
        public async Task<TenantSettingsForEditResponse> GetSettingsForEdit(TenantSettingsGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<int> UpdateAllSettings(TenantSettingsUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<int> SendTestEmail(TenantSettingsSendTestEmailCommand dto)
        {
            return await Mediator.Send(dto);
        }
    }
}
