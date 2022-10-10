using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionUpdateCommandHandler : BaseCoreClass, IRequestHandler<TemplateSectionUpdateCommand, long>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateSectionUpdateCommandHandler(ICoreServices coreServices
        , TemplateManager managerTemplate): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<long> Handle(TemplateSectionUpdateCommand request, CancellationToken cancellationToken)
        {
            TemplateSectionDto dto = new TemplateSectionDto()
            {
                Id = request.Id,
                Template = request.Template,
                Name = request.Name,
                Order = request.Order,
                IconAF = request.IconAF
            };

            await _managerTemplate.UpdateTemplateSectionAsync(dto);

            return dto.Id.Value;
        }
    }
}
