using AlgoriaCore.Application.QueriesAndCommands.Settings.Host;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Domain.MultiTenancy;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Host_Settings, MultiTenancySide = (byte)MultiTenancySides.Host)]
    public class HostSettingsController : BaseController
    {
        [HttpPost]
        public async Task<HostSettingsForEditResponse> GetSettingsForEdit(HostSettingsGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<int> UpdateAllSettings(HostSettingsUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<int> SendTestEmail(HostSettingsSendTestEmailCommand dto)
        {
            return await Mediator.Send(dto);
        }
    }
}
