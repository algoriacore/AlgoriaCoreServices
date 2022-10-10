using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateGetForEditQueryHandler : BaseCoreClass, IRequestHandler<TemplateGetForEditQuery, TemplateForEditResponse>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateGetForEditQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ) : base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<TemplateForEditResponse> Handle(TemplateGetForEditQuery request, CancellationToken cancellationToken)
        {
            TemplateForEditResponse response;

            if (request.Id.HasValue)
            {
                TemplateDto dto = await _managerTemplate.GetTemplateAsync(request.Id.Value);

                response = new TemplateForEditResponse()
                {
                    Id = dto.Id,
                    RGBColor = dto.RGBColor,
                    NameSingular = dto.NameSingular,
                    NamePlural = dto.NamePlural,
                    Description = dto.Description,
                    Icon = dto.Icon,
                    IsTableGenerated = dto.IsTableGenerated,
                    TableName = dto.TableName,
                    HasChatRoom = dto.HasChatRoom,
                    IsActivity = dto.IsActivity,
                    HasSecurity = dto.HasSecurity,
                    IsActive = dto.IsActive
                };
            } else {
                response = new TemplateForEditResponse();
            }

            return response;
        }
    }
}
