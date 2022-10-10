using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Processes
{
    public class ProcessDeleteCommandHandler : BaseCoreClass, IRequestHandler<ProcessDeleteCommand, long>
    {
        private readonly ProcessManager _manager;

        public ProcessDeleteCommandHandler(ICoreServices coreServices, ProcessManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(ProcessDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.SetTemplate(request.Template);
            await _manager.ValidateProcessEditPermission(request.Id);
            await _manager.DeleteProcessAsync(request.Id);

            return request.Id;
        }
    }
}
