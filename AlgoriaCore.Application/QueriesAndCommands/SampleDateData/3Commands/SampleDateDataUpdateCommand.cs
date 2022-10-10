using MediatR;
using System;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataUpdateCommand : IRequest<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateTimeData { get; set; }
        public DateTime? DateData { get; set; }
        public TimeSpan? TimeData { get; set; }
    }
}