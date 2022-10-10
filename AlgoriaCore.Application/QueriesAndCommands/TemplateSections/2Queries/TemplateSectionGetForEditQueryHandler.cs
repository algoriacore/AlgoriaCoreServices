using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionGetForEditQueryHandler : BaseCoreClass, IRequestHandler<TemplateSectionGetForEditQuery, TemplateSectionForEditResponse>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateSectionGetForEditQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ) : base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<TemplateSectionForEditResponse> Handle(TemplateSectionGetForEditQuery request, CancellationToken cancellationToken)
        {
            TemplateSectionForEditResponse response;

            if (request.Id.HasValue)
            {
                TemplateSectionDto dto = await _managerTemplate.GetTemplateSectionAsync(request.Id.Value);

                response = new TemplateSectionForEditResponse()
                {
                    Id = dto.Id,
                    Template = dto.Template,
                    Name = dto.Name,
                    Order = dto.Order,
                    IconAF = dto.IconAF
                };
            } else {
                response = new TemplateSectionForEditResponse();
            }

            return response;
        }
    }
}
