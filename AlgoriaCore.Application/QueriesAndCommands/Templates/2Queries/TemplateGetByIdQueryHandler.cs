using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Templates
{
    public class TemplateGetByIdQueryHandler : BaseCoreClass, IRequestHandler<TemplateGetByIdQuery, TemplateResponse>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateGetByIdQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<TemplateResponse> Handle(TemplateGetByIdQuery request, CancellationToken cancellationToken)
        {
            TemplateResponse response = null;
            TemplateDto dto = await _managerTemplate.GetTemplateAsync(request.Id);

            response = new TemplateResponse()
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

            return response;
        }
    }
}
