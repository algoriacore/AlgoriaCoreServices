using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants
{
    public class TenantExportCSVQuery : PageListByDto, IRequest<FileDto>
    {
        public string ViewColumnsConfigJSON { get; set; }
    }
}
