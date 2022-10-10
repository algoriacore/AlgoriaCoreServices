using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusGetForEditQueryHandler : BaseCoreClass, IRequestHandler<TemplateToDoStatusGetForEditQuery, TemplateToDoStatusForEditResponse>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateToDoStatusGetForEditQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ) : base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<TemplateToDoStatusForEditResponse> Handle(TemplateToDoStatusGetForEditQuery request, CancellationToken cancellationToken)
        {
            TemplateToDoStatusForEditResponse response;

            if (request.Id.HasValue)
            {
                TemplateToDoStatusDto dto = await _managerTemplate.GetTemplateToDoStatusAsync(request.Id.Value);

                response = new TemplateToDoStatusForEditResponse()
                {
                    Id = dto.Id.Value,
                    Template = dto.Template,
                    Type = dto.Type,
                    Name = dto.Name,
                    IsDefault = dto.IsDefault,
                    IsActive = dto.IsActive
                };
            } else {
                response = new TemplateToDoStatusForEditResponse();
            }

            return response;
        }
    }
}
