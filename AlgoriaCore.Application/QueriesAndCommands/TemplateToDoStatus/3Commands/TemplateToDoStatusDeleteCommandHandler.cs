using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusDeleteCommandHandler : BaseCoreClass, IRequestHandler<TemplateToDoStatusDeleteCommand, long>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateToDoStatusDeleteCommandHandler(ICoreServices coreServices
        , TemplateManager managerTemplate): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<long> Handle(TemplateToDoStatusDeleteCommand request, CancellationToken cancellationToken)
        {
            await _managerTemplate.DeleteTemplateToDoStatusAsync(request.Id);

            return request.Id;
        }
    }
}
