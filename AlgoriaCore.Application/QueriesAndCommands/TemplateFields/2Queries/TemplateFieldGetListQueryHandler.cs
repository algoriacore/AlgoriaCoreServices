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
    public class TemplateFieldGetListQueryHandler : BaseCoreClass, IRequestHandler<TemplateFieldGetListQuery, PagedResultDto<TemplateFieldForListResponse>>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateFieldGetListQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<PagedResultDto<TemplateFieldForListResponse>> Handle(TemplateFieldGetListQuery request, CancellationToken cancellationToken)
        {
            TemplateFieldListFilterDto filterDto = new TemplateFieldListFilterDto()
            {
                Filter = request.Filter,
                IsPaged = request.IsPaged,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                OnlyProcessed = request.OnlyProcessed,
                Template = request.Template,
                TemplateSection = request.TemplateSection
            };

            PagedResultDto<TemplateFieldDto> pagedResultDto = await _managerTemplate.GetTemplateFieldListAsync(filterDto);
            List<TemplateFieldForListResponse> ll = new List<TemplateFieldForListResponse>();

            foreach (TemplateFieldDto dto in pagedResultDto.Items)
            {
                ll.Add(new TemplateFieldForListResponse()
                {
                    Id = dto.Id.Value,
                    TemplateSection = dto.TemplateSection,
                    TemplateSectionDesc = dto.TemplateSectionDesc,
                    TemplateSectionIconAF = dto.TemplateSectionIconAF,
                    TemplateSectionOrder = dto.TemplateSectionOrder,
                    Name = dto.Name,
                    FieldName = dto.FieldName,
                    FieldTypeDesc = dto.FieldTypeDesc,
                    FieldSize = dto.FieldSize,
                    FieldControlDesc = dto.FieldControlDesc,
                    InputMask = dto.InputMask,
                    KeyFilter = dto.KeyFilter,
                    Status = dto.Status.Value,
                    StatusDesc = dto.StatusDesc,
                    IsRequiredDesc = dto.IsRequiredDesc,
                    ShowOnGridDesc = dto.ShowOnGridDesc,
                    Order = dto.Order,
                    MustHaveOptions = dto.MustHaveOptions
                });
            }

            return new PagedResultDto<TemplateFieldForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
