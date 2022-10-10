using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldDeleteCommandHandler : BaseCoreClass, IRequestHandler<TemplateFieldDeleteCommand, long>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateFieldDeleteCommandHandler(ICoreServices coreServices
        , TemplateManager managerTemplate): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<long> Handle(TemplateFieldDeleteCommand request, CancellationToken cancellationToken)
        {
            await _managerTemplate.DeleteTemplateFieldAsync(request.Id);

            return request.Id;
        }
    }
}
