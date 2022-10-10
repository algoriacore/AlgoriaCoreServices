using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetByIdQueryHandler : BaseCoreClass, IRequestHandler<ProcessGetByIdQuery, ProcessResponse>
    {
        private readonly ProcessManager _manager;

        public ProcessGetByIdQueryHandler(ICoreServices coreServices, ProcessManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<ProcessResponse> Handle(ProcessGetByIdQuery request, CancellationToken cancellationToken)
        {
            await _manager.SetTemplate(request.Template);
            await _manager.ValidateProcessReadPermission(request.Id);

            ProcessDto dto = await _manager.GetProcessAsync(request.Id);

            return new ProcessResponse()
            {
                Id = dto.Id,
                Data = dto.DataFromServer,
                Activity = dto.Activity == null? null: new ProcessToDoActivityResponse() {
                    Id = dto.Activity.Id,
                    UserCreator = dto.Activity.UserCreator,
                    Status = dto.Activity.Status,
                    CreationTime = dto.Activity.CreationTime,
                    Description = dto.Activity.Description,
                    InitialPlannedDate = dto.Activity.InitialPlannedDate,
                    FinalPlannedDate = dto.Activity.FinalPlannedDate,
                    InitialRealDate = dto.Activity.InitialRealDate,
                    FinalRealDate = dto.Activity.FinalRealDate,
                    IsOnTime = dto.Activity.IsOnTime,
                    Executor = dto.Activity.Executor,
                    Evaluator = dto.Activity.Evaluator
                }
            };
        }
    }
}
