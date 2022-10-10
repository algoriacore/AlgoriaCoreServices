using MediatR;
using System;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataConvertCommand : IRequest<DateTime>
    {
        public string TimeZoneFrom { get; set; }
        public string TimeZoneTo { get; set; }
        public DateTime? DateTimeDataToConvert { get; set; }
    }
}