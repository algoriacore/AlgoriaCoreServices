using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldGetByIdQueryHandler : BaseCoreClass, IRequestHandler<TemplateFieldGetByIdQuery, TemplateFieldResponse>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateFieldGetByIdQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<TemplateFieldResponse> Handle(TemplateFieldGetByIdQuery request, CancellationToken cancellationToken)
        {
            TemplateFieldResponse response = null;
            TemplateFieldDto dto = await _managerTemplate.GetTemplateFieldAsync(request.Id);

            response = new TemplateFieldResponse()
            {
                Id = dto.Id.Value,
                TemplateSection = dto.TemplateSection,
                Name = dto.Name,
                FieldName = dto.FieldName,
                FieldType = dto.FieldType,
                FieldSize = dto.FieldSize,
                FieldControl = dto.FieldControl,
                InputMask = dto.InputMask,
                HasKeyFilter = dto.HasKeyFilter,
                KeyFilter = dto.KeyFilter,
                Status = dto.Status,
                IsRequired = dto.IsRequired,
                ShowOnGrid = dto.ShowOnGrid,
                Order = dto.Order,
                InheritSecurity = dto.InheritSecurity,
                TemplateFieldRelationTemplate = dto.TemplateFieldRelationTemplate,
                TemplateFieldRelationTemplateField = dto.TemplateFieldRelationTemplateField,
                Options = dto.Options
            };

            return response;
        }
    }
}
