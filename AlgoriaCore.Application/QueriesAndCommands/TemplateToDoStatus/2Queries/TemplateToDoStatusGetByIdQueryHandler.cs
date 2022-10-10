using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusGetByIdQueryHandler : BaseCoreClass, IRequestHandler<TemplateToDoStatusGetByIdQuery, TemplateToDoStatusResponse>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateToDoStatusGetByIdQueryHandler(ICoreServices coreServices, TemplateManager managerTemplate): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<TemplateToDoStatusResponse> Handle(TemplateToDoStatusGetByIdQuery request, CancellationToken cancellationToken)
        {
            TemplateToDoStatusResponse response = null;
            TemplateToDoStatusDto dto = await _managerTemplate.GetTemplateToDoStatusAsync(request.Id);

            response = new TemplateToDoStatusResponse()
            {
                Id = dto.Id.Value,
                Template = dto.Template,
                Type = dto.Type,
                Name = dto.Name,
                IsDefault = dto.IsDefault,
                IsActive = dto.IsActive
            };

            return response;
        }
    }
}
