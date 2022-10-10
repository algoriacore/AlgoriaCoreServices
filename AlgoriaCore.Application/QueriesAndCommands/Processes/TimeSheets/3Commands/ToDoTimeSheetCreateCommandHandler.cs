using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetCreateCommandHandler : BaseCoreClass, IRequestHandler<ToDoTimeSheetCreateCommand, long>
    {
        private readonly ProcessManager _manager;

        public ToDoTimeSheetCreateCommandHandler(ICoreServices coreServices, ProcessManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(ToDoTimeSheetCreateCommand request, CancellationToken cancellationToken)
        {
            ToDoTimeSheetDto dto = new ToDoTimeSheetDto()
            {
                Activity = request.Activity,
                CreationDate = request.CreationDate,
                Comments = request.Comments,
                HoursSpend = request.HoursSpend,
                ActivityStatus = request.ActivityStatus
            };

            return await _manager.CreateToDoTimeSheetAsync(dto);
        }
    }
}
