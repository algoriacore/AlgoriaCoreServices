using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Helps;
using AlgoriaCore.Application.Managers.Helps.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpGetListQueryHandler : BaseCoreClass, IRequestHandler<HelpGetListQuery, PagedResultDto<HelpForListResponse>>
    {
        private readonly HelpManager _manager;

        public HelpGetListQueryHandler(
            ICoreServices coreServices,
            HelpManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<HelpForListResponse>> Handle(HelpGetListQuery request, CancellationToken cancellationToken)
        {
            HelpListFilterDto filterDto = new HelpListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                IsIncludeBody = false
            };

            PagedResultDto<HelpDto> pagedResultDto = await _manager.GetHelpListAsync(filterDto);
            List<HelpForListResponse> ll = new List<HelpForListResponse>();

            foreach (HelpDto dto in pagedResultDto.Items)
            {
                ll.Add(new HelpForListResponse()
                {
                    Id = dto.Id.Value,
                    LanguageDesc = dto.LanguageDesc,
                    Key = dto.Key,
                    DisplayName = dto.DisplayName,
                    IsActive = dto.IsActive,
                    IsActiveDesc = dto.IsActiveDesc
                });
            }

            return new PagedResultDto<HelpForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
