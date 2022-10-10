using AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [Route("api/register")]
    public class RegisterController : BaseController
    {
        [AllowAnonymous]
        [HttpPost, Route("create")]
        public async Task<string> Create([FromBody]TenantCreateRegistrationCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AllowAnonymous]
        [HttpPost, Route("confirm")]
        public async Task<int> Confirm([FromBody]TenantConfirmRegistrationCommand dto)
        {
            return await Mediator.Send(dto);
        }
    }
}
