using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetUpdateCommandHandler : BaseCoreClass, IRequestHandler<ToDoTimeSheetUpdateCommand, long>
    {
        private readonly ProcessManager _managerToDoTimeSheet;

        public ToDoTimeSheetUpdateCommandHandler(ICoreServices coreServices, ProcessManager managerToDoTimeSheet): base(coreServices)
        {
            _managerToDoTimeSheet = managerToDoTimeSheet;
        }

        public async Task<long> Handle(ToDoTimeSheetUpdateCommand request, CancellationToken cancellationToken)
        {
            ToDoTimeSheetDto dto = new ToDoTimeSheetDto()
            {
                Id = request.Id,
                CreationDate = request.CreationDate,
                Comments = request.Comments,
                HoursSpend = request.HoursSpend,
                ActivityStatus = request.ActivityStatus
            };

            await _managerToDoTimeSheet.UpdateToDoTimeSheetAsync(dto);

            return dto.Id.Value;
        }
    }
}
