using MediatR;
using System;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleDateData
{
    public class SampleDateDataCreateCommand : IRequest<long>
    {
        public string Name { get; set; }
        public DateTime? DateTimeData { get; set; }
        public DateTime? DateData { get; set; }
        public TimeSpan? TimeData { get; set; }
    }
}