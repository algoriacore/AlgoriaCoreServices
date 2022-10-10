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
    public class ProcessCreateCommandHandler : BaseCoreClass, IRequestHandler<ProcessCreateCommand, long>
    {
        private readonly ProcessManager _manager;

        public ProcessCreateCommandHandler(ICoreServices coreServices, ProcessManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(ProcessCreateCommand request, CancellationToken cancellationToken)
        {
            await _manager.SetTemplate(request.Template);

            ProcessDto dto = new ProcessDto()
            {
                DataFromClient = request.Data,
                Activity = request.Activity == null ? null : new ToDoActivityDto()
                {
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

            return await _manager.CreateProcessAsync(dto);
        }
    }
}
