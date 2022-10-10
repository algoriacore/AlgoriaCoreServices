using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusGetListQuery : PageListByDto, IRequest<PagedResultDto<TemplateToDoStatusForListResponse>>
    {
        public long? Template { get; set; }
    }
}
