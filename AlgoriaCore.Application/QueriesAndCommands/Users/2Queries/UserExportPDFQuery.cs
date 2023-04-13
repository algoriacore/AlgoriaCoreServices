using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserExportPDFQuery : PageListByDto, IRequest<FileDto>
    {
        public int? Tenant { get; set; }
        public string ViewColumnsConfigJSON { get; set; }

    }
}
