using AlgoriaCore.Application.BaseClases.Dto;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.ChangeLogs
{
    public class ChangeLogGetListQuery : PageListByDto, IRequest<PagedResultDto<ChangeLogForListResponse>>
    {
        public string Table { get; set; }
        public string Key { get; set; }
    }
}
