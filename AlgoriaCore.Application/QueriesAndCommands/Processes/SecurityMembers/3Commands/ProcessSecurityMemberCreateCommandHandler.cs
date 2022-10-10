using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Processes;
using AlgoriaCore.Application.Managers.Processes.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SecurityMembers
{
    public class ProcessSecurityMemberCreateCommandHandler : BaseCoreClass, IRequestHandler<ProcessSecurityMemberCreateCommand, long>
    {
        private readonly ProcessManager _manager;

        public ProcessSecurityMemberCreateCommandHandler(ICoreServices coreServices, ProcessManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(ProcessSecurityMemberCreateCommand request, CancellationToken cancellationToken)
        {
            await _manager.SetTemplate(request.Template);

            ProcessSecurityMemberDto dto = new ProcessSecurityMemberDto()
            {
                Parent = request.Parent,
                Type = request.Type,
                Member = request.Member,
                Level = request.Level,
                IsExecutor = request.IsExecutor
            };

            return await _manager.CreateProcessSecurityMemberAsync(dto);
        }
    }
}
