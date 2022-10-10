using AlgoriaCore.Domain.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Filters
{
    public class SessionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
            )
        {
            var services = context.HttpContext.RequestServices;
            var session = services.GetService(typeof(IAppSession)) as IAppSession;

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                try
                {
                    var tenantId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "TenantId").Value;
                    var tenancyName = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "TenancyName").Value;
                    var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
                    var userName = context.HttpContext.User.Identity.Name;
                    var isImpersonalized = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "IsImpersonalized").Value;
                    var impersonalizerUserId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ImpersonalizerUserId").Value;

                    session.TenantId = !string.IsNullOrEmpty(tenantId) ? int.Parse(tenantId) : (int?)null;
                    session.TenancyName = tenancyName;
                    session.UserId = !string.IsNullOrEmpty(userId) ? long.Parse(userId) : (long?)null;
                    session.UserName = userName;
                    session.IsImpersonalized = isImpersonalized.ToLower() == "true";
                    session.ImpersonalizerUserId = !string.IsNullOrEmpty(impersonalizerUserId) ? long.Parse(impersonalizerUserId) : (long?)null;
                    session.TimeZone = context.HttpContext.Request.Headers["The-Timezone-IANA"].FirstOrDefault();
                }
                catch (Exception) //Un error ocurrió. Hay un problema en la obtención de la información de sesión.
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            else
            {
                session.UserName = "";
                session.TenantId = null;
            }

            await next();
        }
    }
}
