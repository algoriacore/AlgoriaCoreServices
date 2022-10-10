using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateFields
{
    public class TemplateFieldGetListByTemplateQueryHandler : BaseCoreClass, IRequestHandler<TemplateFieldGetListByTemplateQuery, List<TemplateFieldResponse>>
    {
        private readonly TemplateManager _manager;

        public TemplateFieldGetListByTemplateQueryHandler(ICoreServices coreServices
        , TemplateManager manager
        ): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<List<TemplateFieldResponse>> Handle(TemplateFieldGetListByTemplateQuery request, CancellationToken cancellationToken)
        {
            List<TemplateFieldDto> list = await _manager.GetTemplateFieldByTemplateListAsync(request.Template, request.OnlyProcessed);
            List<TemplateFieldResponse> ll = new List<TemplateFieldResponse>();

            foreach (TemplateFieldDto dto in list)
            {
                ll.Add(new TemplateFieldResponse()
                {
                    Id = dto.Id.Value,
                    TemplateSection = dto.TemplateSection,
                    TemplateSectionDesc = dto.TemplateSectionDesc,
                    TemplateSectionIconAF = dto.TemplateSectionIconAF,
                    TemplateSectionOrder = dto.TemplateSectionOrder,
                    Name = dto.Name,
                    FieldName = dto.FieldName,
                    FieldType = dto.FieldType,
                    FieldSize = dto.FieldSize,
                    FieldControl= dto.FieldControl,
                    InputMask = dto.InputMask,
                    HasKeyFilter = dto.HasKeyFilter,
                    KeyFilter = dto.KeyFilter,
                    Status = dto.Status,
                    IsRequired = dto.IsRequired,
                    ShowOnGrid = dto.ShowOnGrid,
                    Order = dto.Order,
                    InheritSecurity = dto.InheritSecurity,
                    Options = dto.Options,
                    TemplateFieldRelationTemplateField = dto.TemplateFieldRelationTemplateField,
                    MustHaveOptions = dto.MustHaveOptions
                });
            }

            return ll;
        }
    }
}
