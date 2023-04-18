using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpExportQuery : PageListByDto, IRequest<FileDto>
    {
        public string ViewColumnsConfigJSON { get; set; }

    }
}
