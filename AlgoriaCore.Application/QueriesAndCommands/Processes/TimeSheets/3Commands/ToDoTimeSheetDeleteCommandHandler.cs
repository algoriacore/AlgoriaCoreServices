using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes.TimeSheets
{
    public class ToDoTimeSheetDeleteCommandHandler : BaseCoreClass, IRequestHandler<ToDoTimeSheetDeleteCommand, long>
    {
        private readonly ProcessManager _manager;

        public ToDoTimeSheetDeleteCommandHandler(ICoreServices coreServices, ProcessManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(ToDoTimeSheetDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.DeleteToDoTimeSheetAsync(request.Id);

            return request.Id;
        }
    }
}
