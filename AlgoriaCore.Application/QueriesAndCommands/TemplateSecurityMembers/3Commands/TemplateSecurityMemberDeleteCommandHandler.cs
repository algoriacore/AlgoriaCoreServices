using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers
{
    public class TemplateSecurityMemberDeleteCommandHandler : BaseCoreClass, IRequestHandler<TemplateSecurityMemberDeleteCommand, long>
    {
        private readonly TemplateManager _manager;

        public TemplateSecurityMemberDeleteCommandHandler(ICoreServices coreServices, TemplateManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(TemplateSecurityMemberDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.DeleteTemplateSecurityMemberAsync(request.Id, request.Type, request.Level);

            return request.Id;
        }
    }
}
