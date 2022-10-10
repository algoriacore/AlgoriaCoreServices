using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSections
{
    public class TemplateSectionGetListQueryHandler : BaseCoreClass, IRequestHandler<TemplateSectionGetListQuery, PagedResultDto<TemplateSectionForListResponse>>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateSectionGetListQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<PagedResultDto<TemplateSectionForListResponse>> Handle(TemplateSectionGetListQuery request, CancellationToken cancellationToken)
        {
            TemplateSectionListFilterDto filterDto = new TemplateSectionListFilterDto()
            {
                Filter = request.Filter,
                IsPaged = request.IsPaged,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                Template = request.Template
            };

            PagedResultDto<TemplateSectionDto> pagedResultDto = await _managerTemplate.GetTemplateSectionListAsync(filterDto);
            List<TemplateSectionForListResponse> ll = new List<TemplateSectionForListResponse>();

            foreach (TemplateSectionDto dto in pagedResultDto.Items)
            {
                ll.Add(new TemplateSectionForListResponse()
                {
                    Id = dto.Id.Value,
                    Template = dto.Template,
                    TemplateDesc = dto.TemplateDesc,
                    Name = dto.Name,
                    Order = dto.Order,
                    IconAF = dto.IconAF
                });
            }
            return new PagedResultDto<TemplateSectionForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
