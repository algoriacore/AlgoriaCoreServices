using AlgoriaCore.Application.QueriesAndCommands.Logging._3Commands;
using AlgoriaCore.WebUI.Controllers;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebAPI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class SampleLogController : BaseController
    {
        [HttpPost]
        public async Task<bool> CreateSampleLogTrace([FromBody]SampleLogTraceCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<bool> CreateSampleLogDebug([FromBody]SampleLogDebugCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<bool> CreateSampleLogInformation([FromBody]SampleLogInformationCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<bool> CreateSampleLogWarning([FromBody]SampleLogWarningCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<bool> CreateSampleLogError([FromBody]SampleLogErrorCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<bool> CreateSampleLogCritical([FromBody]SampleLogCriticalCommand dto)
        {
            return await Mediator.Send(dto);
        }
    }
}