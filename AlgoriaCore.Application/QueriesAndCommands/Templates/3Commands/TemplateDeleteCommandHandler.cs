using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateDeleteCommandHandler : BaseCoreClass, IRequestHandler<TemplateDeleteCommand, long>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateDeleteCommandHandler(ICoreServices coreServices
        , TemplateManager managerTemplate): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<long> Handle(TemplateDeleteCommand request, CancellationToken cancellationToken)
        {
            await _managerTemplate.DeleteTemplateAsync(request.Id);

            return request.Id;
        }
    }
}
