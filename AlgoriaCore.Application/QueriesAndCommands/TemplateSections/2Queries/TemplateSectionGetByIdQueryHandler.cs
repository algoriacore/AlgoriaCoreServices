using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionGetByIdQueryHandler : BaseCoreClass, IRequestHandler<TemplateSectionGetByIdQuery, TemplateSectionResponse>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateSectionGetByIdQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<TemplateSectionResponse> Handle(TemplateSectionGetByIdQuery request, CancellationToken cancellationToken)
        {
            TemplateSectionResponse response = null;
            TemplateSectionDto dto = await _managerTemplate.GetTemplateSectionAsync(request.Id);

            response = new TemplateSectionResponse()
            {
                Id = dto.Id.Value,
                Template = dto.Template,
                TemplateDesc = dto.TemplateDesc,
                Name = dto.Name,
                Order = dto.Order,
                IconAF = dto.IconAF
            };

            return response;
        }
    }
}
