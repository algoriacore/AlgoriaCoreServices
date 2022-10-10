using AlgoriaCore.Application.QueriesAndCommands.Authorization.Permissions;
using AlgoriaCore.Domain.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [Authorize]
    public class PermissionController : BaseController
    {
        [HttpPost]
        public async Task<Permission> GetPermissionsTree()
        {
            return await Mediator.Send(new PermissionGetTreeQuery());
        }
    }
}
