using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.SamplesDateData;
using AlgoriaCore.Application.Managers.SamplesDateData.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataGetForEditQueryHandler : BaseCoreClass, IRequestHandler<SampleDateDataGetForEditQuery, SampleDateDataForEditResponse>
    {
        private readonly SampleDateDataManager _managerSampleDateData;

        public SampleDateDataGetForEditQueryHandler(ICoreServices coreServices
        , SampleDateDataManager managerSampleDateData)
                                : base(coreServices)
        {
            _managerSampleDateData = managerSampleDateData;
        }

        public async Task<SampleDateDataForEditResponse> Handle(SampleDateDataGetForEditQuery request, CancellationToken cancellationToken)
        {
            SampleDateDataForEditResponse response;

            if (request.Id.HasValue) 
            {
                SampleDateDataDto dto = await _managerSampleDateData.GetSampleDateDataAsync(request.Id.Value);

                response = new SampleDateDataForEditResponse()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    DateTimeData = dto.DateTimeData,
                    DateData = dto.DateData,
                    TimeData = dto.TimeData
                };
            } else 
            {
                response = new SampleDateDataForEditResponse();
            }

            return response;
        }
    }
}
