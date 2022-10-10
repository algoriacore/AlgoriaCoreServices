using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionGetListQuery : PageListByDto, IRequest<PagedResultDto<TemplateSectionForListResponse>>
    {
        public long? Template { get; set; }
    }
}
