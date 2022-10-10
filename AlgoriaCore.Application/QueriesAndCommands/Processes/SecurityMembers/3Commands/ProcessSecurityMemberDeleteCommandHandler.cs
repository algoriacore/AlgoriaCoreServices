using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SecurityMembers
{
    public class ProcessSecurityMemberDeleteCommandHandler : BaseCoreClass, IRequestHandler<ProcessSecurityMemberDeleteCommand, long>
    {
        private readonly ProcessManager _manager;

        public ProcessSecurityMemberDeleteCommandHandler(ICoreServices coreServices, ProcessManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(ProcessSecurityMemberDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.SetTemplate(request.Template);
            await _manager.DeleteProcessSecurityMemberAsync(request.Id, request.Type, request.Level);

            return request.Id;
        }
    }
}
