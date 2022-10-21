using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.QueriesAndCommands.TemplateFields;
using AlgoriaCore.Application.QueriesAndCommands.Templates;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetForReadQueryHandler : BaseCoreClass, IRequestHandler<ProcessGetForReadQuery, ProcessForReadResponse>
    {
        private readonly IMediator _mediator;
        private readonly ProcessManager _manager;

        public ProcessGetForReadQueryHandler(
            ICoreServices coreServices,
            IMediator mediator,
            ProcessManager manager) : base(coreServices)
        {
            _mediator = mediator;
            _manager = manager;
        }

        public async Task<ProcessForReadResponse> Handle(ProcessGetForReadQuery request, CancellationToken cancellationToken)
        {
            ProcessForReadResponse response;

            await _manager.SetTemplate(request.Template);
            await _manager.ValidateProcessReadPermission(request.Id);

            ProcessDto dto = await _manager.GetProcessAsync(request.Id);

            response = new ProcessForReadResponse()
            {
                Id = dto.Id,
                Data = dto.DataFromServer,
                Activity = dto.Activity == null ? null : new ProcessToDoActivityForEditResponse()
                {
                    Id = dto.Activity.Id,
                    UserCreator = dto.Activity.UserCreator,
                    UserCreatorDesc = dto.Activity.UserCreatorDesc,
                    Status = dto.Activity.Status,
                    StatusDesc = dto.Activity.StatusDesc,
                    CreationTime = dto.Activity.CreationTime,
                    Description = dto.Activity.Description,
                    InitialPlannedDate = dto.Activity.InitialPlannedDate,
                    FinalPlannedDate = dto.Activity.FinalPlannedDate,
                    InitialRealDate = dto.Activity.InitialRealDate,
                    FinalRealDate = dto.Activity.FinalRealDate,
                    IsOnTime = dto.Activity.IsOnTime,
                    IsOnTimeDesc = dto.Activity.IsOnTimeDesc,
                    Executor = dto.Activity.Executor,
                    Evaluator = dto.Activity.Evaluator
                }
            };

            response.Template = await _mediator.Send(new TemplateGetByIdQuery() { Id = request.Template }, cancellationToken);
            response.TemplateFields = await _mediator.Send(new TemplateFieldGetListByTemplateQuery() { Template = request.Template, OnlyProcessed = true }, cancellationToken);

            return response;
        }
    }
}
