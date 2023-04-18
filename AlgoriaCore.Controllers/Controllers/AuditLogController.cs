using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Auditing;
using AlgoriaCore.Application.QueriesAndCommands.Auditing._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Auditing._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Users._2Queries;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_AuditLogs)]
    public class AuditLogController : BaseController
    {
        [HttpPost]
        public async Task<PagedResultDto<AuditLogListResponse>> GetAuditLogList([FromBody]AuditLogGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<AuditLogExcelResponse> GetAuditLogsToExcel([FromBody]AuditLogGetExcelQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportAuditLog([FromBody] AuditLogExportQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportCSVAuditLog([FromBody] AuditLogExportCSVQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportPDFAuditLog([FromBody] AuditLogExportPDFQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}