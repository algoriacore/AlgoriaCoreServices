using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldNextOrderGetByTemplateQueryHandler : BaseCoreClass, IRequestHandler<TemplateFieldNextOrderGetByTemplateSectionQuery, short>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateFieldNextOrderGetByTemplateQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<short> Handle(TemplateFieldNextOrderGetByTemplateSectionQuery request, CancellationToken cancellationToken)
        {
            return await _managerTemplate.GetTemplateFieldNextOrderByTemplateSection(request.TemplateSection);
        }
    }
}
