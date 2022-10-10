using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionCreateCommandHandler : BaseCoreClass, IRequestHandler<TemplateSectionCreateCommand, long>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateSectionCreateCommandHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<long> Handle(TemplateSectionCreateCommand request, CancellationToken cancellationToken)
        {
            TemplateSectionDto dto = new TemplateSectionDto()
            {
                Template = request.Template,
                Name = request.Name,
                Order = request.Order,
                IconAF = request.IconAF
            };

            return await _managerTemplate.CreateTemplateSectionAsync(dto);
        }
    }
}
