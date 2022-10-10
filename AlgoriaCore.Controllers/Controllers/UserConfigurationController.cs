using AlgoriaCore.Application.QueriesAndCommands.UserConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AllowAnonymous]
    public class UserConfigurationController : BaseController
    {
        [HttpPost]
        public async Task<UserConfigurationResponse> GetAll(UserConfigurationGetAllQuery dto)
        {
            return await Mediator.Send(dto);
        }
    }
}