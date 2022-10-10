using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleData
{
    public class GetSampleDataQueryHandler : BaseCoreClass, IRequestHandler<GetSampleDataQuery, IEnumerable<WeatherForecast>>
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public GetSampleDataQueryHandler(ILogger<GetSampleDataQueryHandler> logger, ICoreServices coreServices)
                                : base(coreServices)
        {
        }

        public async Task<IEnumerable<WeatherForecast>> Handle(GetSampleDataQuery request, CancellationToken cancellationToken)
        {
            var rng = new Random();

            return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }));
        }
    }
}
