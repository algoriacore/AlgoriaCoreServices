using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using AlgoriaCore.Application.QueriesAndCommands.TemplateFields;
using AlgoriaCore.Application.QueriesAndCommands.Templates;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessGetForEditQueryHandler : BaseCoreClass, IRequestHandler<ProcessGetForEditQuery, ProcessForEditResponse>
    {
        private readonly IMediator _mediator;
        private readonly ProcessManager _manager;
        private readonly TemplateManager _managerTemplate;

        public ProcessGetForEditQueryHandler(
            ICoreServices coreServices,
            IMediator mediator,
            ProcessManager manager,
            TemplateManager managerTemplate) : base(coreServices)
        {
            _mediator = mediator;
            _manager = manager;
            _managerTemplate = managerTemplate;
        }

        public async Task<ProcessForEditResponse> Handle(ProcessGetForEditQuery request, CancellationToken cancellationToken)
        {
            ProcessForEditResponse response;

            if (request.Id.HasValue)
            {
                await _manager.SetTemplate(request.Template);
                await _manager.ValidateProcessEditPermission(request.Id.Value);

                ProcessDto dto = await _manager.GetProcessAsync(request.Id.Value);

                response = new ProcessForEditResponse()
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
            }
            else
            {
                response = new ProcessForEditResponse();
            }

            response.Template = await _mediator.Send(new TemplateGetByIdQuery() { Id = request.Template }, cancellationToken);
            response.TemplateFields = await _mediator.Send(new TemplateFieldGetListByTemplateQuery() { Template = request.Template, OnlyProcessed = true }, cancellationToken);

            if (response.Template.IsActivity)
            {
                response.ActivityStatusCombo = await _managerTemplate.GetTemplateToDoStatusComboAsync(new TemplateToDoStatusComboFilterDto()
                {
                    Template = request.Template,
                    IsActive = true
                });

                if (response.Data != null &&
                    response.ActivityStatusCombo != null &&
                    !response.ActivityStatusCombo.Exists(p => p.Value == response.Activity.Status.ToString()))
                {
                    response.ActivityStatusCombo.Add(new ComboboxItemDto(response.Activity.Status.ToString(), response.Activity.StatusDesc));
                    response.ActivityStatusCombo = response.ActivityStatusCombo.OrderBy(p => p.Label).ToList();
                }
            }

            return response;
        }
    }
}
