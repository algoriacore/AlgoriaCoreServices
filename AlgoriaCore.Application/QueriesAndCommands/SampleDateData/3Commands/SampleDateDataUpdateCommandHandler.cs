using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.SamplesDateData;
using AlgoriaCore.Application.Managers.SamplesDateData.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataUpdateCommandHandler : BaseCoreClass, IRequestHandler<SampleDateDataUpdateCommand, long>
    {
        private readonly SampleDateDataManager _managerSampleDateData;

        public SampleDateDataUpdateCommandHandler(ICoreServices coreServices
        , SampleDateDataManager managerSampleDateData): base(coreServices)
        {
            _managerSampleDateData = managerSampleDateData;
        }

        public async Task<long> Handle(SampleDateDataUpdateCommand request, CancellationToken cancellationToken)
        {
            SampleDateDataDto dto = new SampleDateDataDto()
            {
                Id = request.Id,
                Name = request.Name,
                DateTimeData = request.DateTimeData,
                DateData = request.DateData,
                TimeData = request.TimeData
            };

            await _managerSampleDateData.UpdateSampleDateDataAsync(dto);

            return dto.Id.Value;
        }
    }
}
