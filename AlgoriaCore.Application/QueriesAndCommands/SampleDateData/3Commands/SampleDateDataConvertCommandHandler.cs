using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Extensiones;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.SamplesDateData;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataConvertCommandHandler : BaseCoreClass, IRequestHandler<SampleDateDataConvertCommand, DateTime>
    {
        public SampleDateDataConvertCommandHandler(ICoreServices coreServices
        , SampleDateDataManager managerSampleDateData): base(coreServices)
        {
        }

        public async Task<DateTime> Handle(SampleDateDataConvertCommand request, CancellationToken cancellationToken)
        {
            DateTime dtUTC = request.DateTimeDataToConvert.Value.InZone(request.TimeZoneFrom);
            DateTime dtTo = dtUTC.ToZone(request.TimeZoneTo);

            return await Task.FromResult(dtTo);
        }
    }
}
