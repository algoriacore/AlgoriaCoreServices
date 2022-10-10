using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleData
{
    public class GetSampleDataQuery : IRequest<IEnumerable<WeatherForecast>>
    {
        public string Id { get; set; }
    }
}
