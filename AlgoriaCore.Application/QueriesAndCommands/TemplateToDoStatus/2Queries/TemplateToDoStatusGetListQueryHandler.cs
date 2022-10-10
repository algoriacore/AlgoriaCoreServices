using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusGetListQueryHandler : BaseCoreClass, IRequestHandler<TemplateToDoStatusGetListQuery, PagedResultDto<TemplateToDoStatusForListResponse>>
    {
        private readonly TemplateManager _managerTemplate;

        public TemplateToDoStatusGetListQueryHandler(ICoreServices coreServices
        , TemplateManager managerTemplate
        ): base(coreServices)
        {
            _managerTemplate = managerTemplate;
        }

        public async Task<PagedResultDto<TemplateToDoStatusForListResponse>> Handle(TemplateToDoStatusGetListQuery request, CancellationToken cancellationToken)
        {
            TemplateToDoStatusListFilterDto filterDto = new TemplateToDoStatusListFilterDto()
            {
                Filter = request.Filter,
                IsPaged = request.IsPaged,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                Template = request.Template
            };

            PagedResultDto<TemplateToDoStatusDto> pagedResultDto = await _managerTemplate.GetTemplateToDoStatusListAsync(filterDto);
            List<TemplateToDoStatusForListResponse> ll = new List<TemplateToDoStatusForListResponse>();

            foreach (TemplateToDoStatusDto dto in pagedResultDto.Items)
            {
                ll.Add(new TemplateToDoStatusForListResponse()
                {
                    Id = dto.Id.Value,
                    Template = dto.Template,
                    TemplateDesc = dto.TemplateDesc,
                    Type = dto.Type,
                    TypeDesc = dto.TypeDesc,
                    Name = dto.Name,
                    IsDefault = dto.IsDefault,
                    IsDefaultDesc = dto.IsDefaultDesc,
                    IsActive = dto.IsActive,
                    IsActiveDesc = dto.IsActiveDesc
                });
            }

            return new PagedResultDto<TemplateToDoStatusForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
