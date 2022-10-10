using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldUpdateCommandHandler : BaseCoreClass, IRequestHandler<TemplateFieldUpdateCommand, long>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateFieldUpdateCommandHandler(ICoreServices coreServices
        , TemplateManager managerTemplate): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<long> Handle(TemplateFieldUpdateCommand request, CancellationToken cancellationToken)
        {
            TemplateFieldDto dto = new TemplateFieldDto()
            {
                Id = request.Id,
                TemplateSection = request.TemplateSection,
                Name = request.Name,
                FieldName = request.FieldName,
                FieldType = request.FieldType,
                FieldSize = request.FieldSize,
                FieldControl = request.FieldControl,
                InputMask = request.InputMask,
                HasKeyFilter = request.HasKeyFilter,
                KeyFilter = request.KeyFilter,
                IsRequired = request.IsRequired,
                ShowOnGrid = request.ShowOnGrid,
                Order = request.Order,
                InheritSecurity = request.InheritSecurity,
                TemplateFieldRelationTemplateField = request.TemplateFieldRelationTemplateField,
                Options = request.Options
            };

            await _managerTemplate.UpdateTemplateFieldAsync(dto);

            return dto.Id.Value;
        }
    }
}
