using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.ChangeLogs;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class ChangeLogController : BaseController
    {
        [HttpPost]
        public async Task<PagedResultDto<ChangeLogForListResponse>> GetChangeLogList([FromBody]ChangeLogGetListQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
