using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.SamplesDateData;
using AlgoriaCore.Application.Managers.SamplesDateData.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataGetByIdQueryHandler : BaseCoreClass, IRequestHandler<SampleDateDataGetByIdQuery, SampleDateDataResponse>
    {
        private readonly SampleDateDataManager _managerSampleDateData;

        public SampleDateDataGetByIdQueryHandler(ICoreServices coreServices
        , SampleDateDataManager managerSampleDateData)
                                : base(coreServices)
        {
            _managerSampleDateData = managerSampleDateData;
        }

        public async Task<SampleDateDataResponse> Handle(SampleDateDataGetByIdQuery request, CancellationToken cancellationToken)
        {
            SampleDateDataDto dto = await _managerSampleDateData.GetSampleDateDataAsync(request.Id);

            return new SampleDateDataResponse()
            {
                Id = dto.Id,
                Name = dto.Name,
                DateTimeData = dto.DateTimeData,
                DateData = dto.DateData,
                TimeData = dto.TimeData
            };
        }
    }
}
