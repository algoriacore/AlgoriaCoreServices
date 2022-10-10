using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageGetListQueryHandler : BaseCoreClass, IRequestHandler<LanguageGetListQuery, PagedResultDto<LanguageForListResponse>>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageGetListQueryHandler(ICoreServices coreServices
        , LanguageManager managerLanguage)
                                : base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<PagedResultDto<LanguageForListResponse>> Handle(LanguageGetListQuery request, CancellationToken cancellationToken)
        {
            LanguageListFilterDto filterDto = new LanguageListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            PagedResultDto<LanguageDto> pagedResultDto = await _managerLanguage.GetLanguageListAsync(filterDto);
            List<LanguageForListResponse> ll = new List<LanguageForListResponse>();

            foreach (LanguageDto dto in pagedResultDto.Items)
            {
                ll.Add(new LanguageForListResponse()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    DisplayName = dto.DisplayName,
                    IsActive = dto.IsActive,
                    IsActiveDesc = dto.IsActiveDesc
                });
            }
            return new PagedResultDto<LanguageForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
