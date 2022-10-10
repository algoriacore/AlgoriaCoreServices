using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionDeleteCommandHandler : BaseCoreClass, IRequestHandler<TemplateSectionDeleteCommand, long>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateSectionDeleteCommandHandler(ICoreServices coreServices
        , TemplateManager managerTemplate): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<long> Handle(TemplateSectionDeleteCommand request, CancellationToken cancellationToken)
        {
            await _managerTemplate.DeleteTemplateSectionAsync(request.Id);

            return request.Id;
        }
    }
}
