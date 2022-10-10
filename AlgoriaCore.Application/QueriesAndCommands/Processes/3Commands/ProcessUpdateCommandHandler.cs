using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessUpdateCommandHandler : BaseCoreClass, IRequestHandler<ProcessUpdateCommand, long>
    {
        private readonly ProcessManager _manager;

        public ProcessUpdateCommandHandler(ICoreServices coreServices, ProcessManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(ProcessUpdateCommand request, CancellationToken cancellationToken)
        {
            await _manager.SetTemplate(request.Template);
            await _manager.ValidateProcessEditPermission(request.Id);

            ProcessDto dto = new ProcessDto()
            {
                Id = request.Id,
                DataFromClient = request.Data,
                Activity = request.Activity == null ? null: new ToDoActivityDto() {
                    Status = request.Activity.Status,
                    Description = request.Activity.Description,
                    InitialPlannedDate = request.Activity.InitialPlannedDate,
                    FinalPlannedDate = request.Activity.FinalPlannedDate,
                    InitialRealDate = request.Activity.InitialRealDate,
                    FinalRealDate = request.Activity.FinalRealDate,
                    Executor = request.Activity.Executor.Select(p => new ToDoActivityUserDto() { User = p }).ToList(),
                    Evaluator = request.Activity.Evaluator.Select(p => new ToDoActivityUserDto() { User = p }).ToList()
                }
            };

            await _manager.UpdateProcessAsync(dto);

            return request.Id;
        }
    }
}
