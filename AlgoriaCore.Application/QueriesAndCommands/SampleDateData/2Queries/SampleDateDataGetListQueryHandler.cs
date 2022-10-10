using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.SamplesDateData;
using AlgoriaCore.Application.Managers.SamplesDateData.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataGetListQueryHandler : BaseCoreClass, IRequestHandler<SampleDateDataGetListQuery, PagedResultDto<SampleDateDataForListResponse>>
    {
        private readonly SampleDateDataManager _managerSampleDateData;

        public SampleDateDataGetListQueryHandler(ICoreServices coreServices
        , SampleDateDataManager managerSampleDateData)
                                : base(coreServices)
        {
            _managerSampleDateData = managerSampleDateData;
        }

        public async Task<PagedResultDto<SampleDateDataForListResponse>> Handle(SampleDateDataGetListQuery request, CancellationToken cancellationToken)
        {
            SampleDateDataListFilterDto filterDto = new SampleDateDataListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            PagedResultDto<SampleDateDataDto> pagedResultDto = await _managerSampleDateData.GetSampleDateDataListAsync(filterDto);
            List<SampleDateDataForListResponse> ll = new List<SampleDateDataForListResponse>();

            foreach (SampleDateDataDto dto in pagedResultDto.Items)
            {
                ll.Add(new SampleDateDataForListResponse()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    DateTimeData = dto.DateTimeData,
                    DateData = dto.DateData,
                    TimeData = dto.TimeData
                });
            }

            return new PagedResultDto<SampleDateDataForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
