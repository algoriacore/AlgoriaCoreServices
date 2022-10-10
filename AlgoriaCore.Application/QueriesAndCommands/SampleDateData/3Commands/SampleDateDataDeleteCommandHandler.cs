using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.SamplesDateData;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataDeleteCommandHandler : BaseCoreClass, IRequestHandler<SampleDateDataDeleteCommand, long>
    {
        private readonly SampleDateDataManager _managerSampleDateData;

        public SampleDateDataDeleteCommandHandler(ICoreServices coreServices
        , SampleDateDataManager managerSampleDateData): base(coreServices)
        {
            _managerSampleDateData = managerSampleDateData;
        }

        public async Task<long> Handle(SampleDateDataDeleteCommand request, CancellationToken cancellationToken)
        {
            await _managerSampleDateData.DeleteSampleDateDataAsync(request.Id);

            return request.Id;
        }
    }
}
