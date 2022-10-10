using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldGetListQuery : PageListByDto, IRequest<PagedResultDto<TemplateFieldForListResponse>>
    {
        public bool OnlyProcessed { get; set; }
        public long? Template { get; set; }
        public long? TemplateSection { get; set; }
    }
}
