using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetListQueryHandler : BaseCoreClass, IRequestHandler<ProcessGetListQuery, PagedResultDto<Dictionary<string, object>>>
    {
        private readonly ProcessManager _manager;

        public ProcessGetListQueryHandler(ICoreServices coreServices, ProcessManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<Dictionary<string, object>>> Handle(ProcessGetListQuery request, CancellationToken cancellationToken)
        {
            await _manager.SetTemplate(request.Template);

            ProcessListFilterDto filterDto = new ProcessListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                ViewType = request.ViewType
            };

            return await _manager.GetProcessListAsync(filterDto);
        }
    }
}
