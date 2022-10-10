using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextGetListQueryHandler : BaseCoreClass, IRequestHandler<LanguageTextGetListQuery, PagedResultDto<LanguageTextForListResponse>>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageTextGetListQueryHandler(ICoreServices coreServices
        , LanguageManager managerLanguage)
                                : base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<PagedResultDto<LanguageTextForListResponse>> Handle(LanguageTextGetListQuery request, CancellationToken cancellationToken)
        {
            LanguageTextListFilterDto filterDto = new LanguageTextListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                LanguageId = request.LanguageId
            };

            PagedResultDto<LanguageTextDto> pagedResultDto = await _managerLanguage.GetLanguageTextListAsync(filterDto);
            List<LanguageTextForListResponse> ll = new List<LanguageTextForListResponse>();

            foreach(LanguageTextDto dto in pagedResultDto.Items)
            {
                ll.Add(new LanguageTextForListResponse()
                {
                    Id = dto.Id,
                    LanguageId = dto.LanguageId,
                    Key = dto.Key,
                    Value = dto.Value
                });
            }

            return new PagedResultDto<LanguageTextForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
