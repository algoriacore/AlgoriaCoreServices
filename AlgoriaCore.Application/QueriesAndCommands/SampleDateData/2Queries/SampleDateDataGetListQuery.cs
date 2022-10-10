using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataGetListQuery : PageListByDto, IRequest<PagedResultDto<SampleDateDataForListResponse>>
    {

    }
}
