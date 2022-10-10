using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetGetForEditQueryHandler : BaseCoreClass, IRequestHandler<ToDoTimeSheetGetForEditQuery, ToDoTimeSheetForEditResponse>
    {
        private readonly ProcessManager _manager;
        private readonly TemplateManager _managerTemplate;

        public ToDoTimeSheetGetForEditQueryHandler(ICoreServices coreServices, ProcessManager manager, TemplateManager managerTemplate) : base(coreServices)
        {
            _manager = manager;
            _managerTemplate = managerTemplate;
        }

        public async Task<ToDoTimeSheetForEditResponse> Handle(ToDoTimeSheetGetForEditQuery request, CancellationToken cancellationToken)
        {
            ToDoTimeSheetForEditResponse response;

            if (request.Id.HasValue)
            {
                ToDoTimeSheetDto dto = await _manager.GetToDoTimeSheetAsync(request.Id.Value);

                response = new ToDoTimeSheetForEditResponse()
                {
                    Id = dto.Id,
                    Activity = dto.Activity,
                    UserCreator = dto.UserCreator,
                    UserCreatorDesc = dto.UserCreatorDesc,
                    CreationDate = dto.CreationDate,
                    CreationTime = dto.CreationTime,
                    Comments = dto.Comments,
                    HoursSpend = dto.HoursSpend
                };
            }
            else
            {
                response = new ToDoTimeSheetForEditResponse();
            }

            response.ActivityStatusCombo = await _managerTemplate.GetTemplateToDoStatusComboAsync(new TemplateToDoStatusComboFilterDto()
            {
                Template = request.Template,
                IsActive = true
            });

            return response;
        }
    }
}
