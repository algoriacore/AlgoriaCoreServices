using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetGetByIdQueryHandler : BaseCoreClass, IRequestHandler<ToDoTimeSheetGetByIdQuery, ToDoTimeSheetResponse>
    {
        private readonly ProcessManager _manager;

        public ToDoTimeSheetGetByIdQueryHandler(ICoreServices coreServices, ProcessManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<ToDoTimeSheetResponse> Handle(ToDoTimeSheetGetByIdQuery request, CancellationToken cancellationToken)
        {
            ToDoTimeSheetResponse response = null;
            ToDoTimeSheetDto dto = await _manager.GetToDoTimeSheetAsync(request.Id);

            response = new ToDoTimeSheetResponse()
            {
                Id = dto.Id.Value,
                Activity = dto.Activity,
                UserCreator = dto.UserCreator,
                UserCreatorDesc = dto.UserCreatorDesc,
                CreationDate = dto.CreationDate,
                CreationTime = dto.CreationTime,
                Comments = dto.Comments,
                HoursSpend = dto.HoursSpend
            };

            return response;
        }
    }
}
