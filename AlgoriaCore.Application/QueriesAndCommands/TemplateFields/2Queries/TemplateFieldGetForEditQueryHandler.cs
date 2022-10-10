using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldGetForEditQueryHandler : BaseCoreClass, IRequestHandler<TemplateFieldGetForEditQuery, TemplateFieldForEditResponse>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateFieldGetForEditQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ) : base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<TemplateFieldForEditResponse> Handle(TemplateFieldGetForEditQuery request, CancellationToken cancellationToken)
        {
            TemplateFieldForEditResponse response;

            if (request.Id.HasValue)
            {
                TemplateFieldDto dto = await _managerTemplate.GetTemplateFieldAsync(request.Id.Value);

                response = new TemplateFieldForEditResponse()
                {
                    Id = dto.Id,
                    TemplateSection = dto.TemplateSection,
                    TemplateSectionDesc = dto.TemplateSectionDesc,
                    TemplateSectionIconAF = dto.TemplateSectionIconAF,
                    Name = dto.Name,
                    FieldName = dto.FieldName,
                    FieldType = dto.FieldType,
                    FieldSize = dto.FieldSize,
                    FieldControl = dto.FieldControl,
                    InputMask = dto.InputMask,
                    HasKeyFilter = dto.HasKeyFilter,
                    KeyFilter = dto.KeyFilter,
                    Status = dto.Status,
                    StatusDesc = dto.StatusDesc,
                    IsRequired = dto.IsRequired,
                    ShowOnGrid = dto.ShowOnGrid,
                    Order = dto.Order,
                    InheritSecurity = dto.InheritSecurity,
                    TemplateFieldRelationTemplate = dto.TemplateFieldRelationTemplate,
                    TemplateFieldRelationTemplateDesc = dto.TemplateFieldRelationTemplateDesc,
                    TemplateFieldRelationTemplateField = dto.TemplateFieldRelationTemplateField,
                    TemplateFieldRelationTemplateFieldDesc = dto.TemplateFieldRelationTemplateFieldDesc,
                    Options = dto.Options
                };
            } else {
                response = new TemplateFieldForEditResponse();
            }

            response.TemplateCombo = await _managerTemplate.GetTemplateComboAsync();

            if (response.TemplateFieldRelationTemplate != null)
            {
                if (!response.TemplateCombo.Exists(p => p.Value == response.TemplateFieldRelationTemplate.ToString()))
                {
                    response.TemplateCombo.Add(new ComboboxItemDto(response.TemplateFieldRelationTemplate.ToString(), response.TemplateFieldRelationTemplateDesc));
                    response.TemplateCombo = response.TemplateCombo.OrderBy(p => p.Label).ToList();
                }

                response.TemplateFieldCombo = await _managerTemplate.GetTemplateFieldComboAsync(
                    new TemplateFieldComboFilterDto() { Template = response.TemplateFieldRelationTemplate.Value }
                );

                if (response.TemplateFieldRelationTemplateField != null && !response.TemplateFieldCombo.Exists(p => p.Value == response.TemplateFieldRelationTemplateField.ToString()))
                {
                    response.TemplateFieldCombo.Add(new ComboboxItemDto(response.TemplateFieldRelationTemplateField.ToString(), response.TemplateFieldRelationTemplateFieldDesc));
                    response.TemplateFieldCombo = response.TemplateFieldCombo.OrderBy(p => p.Label).ToList();
                }
            }

            return response;
        }
    }
}
