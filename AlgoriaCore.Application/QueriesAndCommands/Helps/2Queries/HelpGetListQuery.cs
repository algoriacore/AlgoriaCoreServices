using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpGetListQuery : PageListByDto, IRequest<PagedResultDto<HelpForListResponse>>
    {

    }
}
