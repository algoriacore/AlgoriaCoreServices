using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Processes.Dto;
using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetListQuery : PageListByDto, IRequest<PagedResultDto<Dictionary<string, object>>>
    {
        public long Template { get; set; }
        public ProcessViewType ViewType { get; set; }

        public ProcessGetListQuery()
        {
            ViewType = ProcessViewType.Normal;
        }
    }
}
