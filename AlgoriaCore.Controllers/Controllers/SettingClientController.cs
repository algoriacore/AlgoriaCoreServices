using AlgoriaCore.Application.QueriesAndCommands.SettingsClient;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class SettingClientController : BaseController
    {
        [HttpPost]
        public async Task<long> ChangeSettingClient([FromBody]SettingClientChangeCommand dto)
        {
            return await Mediator.Send(dto);
        }
    }
}
