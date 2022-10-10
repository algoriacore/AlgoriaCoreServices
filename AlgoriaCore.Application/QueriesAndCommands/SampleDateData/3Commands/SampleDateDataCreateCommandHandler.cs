using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.SamplesDateData;
using AlgoriaCore.Application.Managers.SamplesDateData.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataCreateCommandHandler : BaseCoreClass, IRequestHandler<SampleDateDataCreateCommand, long>
    {
        private readonly SampleDateDataManager _managerSampleDateData;

        public SampleDateDataCreateCommandHandler(ICoreServices coreServices
        , SampleDateDataManager managerSampleDateData) : base(coreServices)
        {
            _managerSampleDateData = managerSampleDateData;
        }

        public async Task<long> Handle(SampleDateDataCreateCommand request, CancellationToken cancellationToken)
        {
            SampleDateDataDto dto = new SampleDateDataDto()
            {
                Name = request.Name,
                DateTimeData = request.DateTimeData,
                DateData = request.DateData,
                TimeData = request.TimeData
            };

            return await _managerSampleDateData.CreateSampleDateDataAsync(dto);
        }
    }
}
