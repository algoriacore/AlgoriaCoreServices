using AlgoriaCore.Application.QueriesAndCommands.Logging._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Logging._2Queries;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Host_Maintenance)]
    public class WebLogController : BaseController
    {
        [HttpPost]
        public async Task<WebLogGetLastestResponse> GetLatestWebLogs([FromBody]WebLogGetLastestQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<WebLogDownloadZipResponse> DownloadWebLogs([FromBody]WebLogDownloadZipQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}