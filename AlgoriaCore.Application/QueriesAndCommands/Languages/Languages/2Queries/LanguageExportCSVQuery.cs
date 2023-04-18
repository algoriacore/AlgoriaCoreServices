using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages
{
    public class LanguageExportCSVQuery : PageListByDto, IRequest<FileDto>
    {
        public string ViewColumnsConfigJSON { get; set; }
    }
}
