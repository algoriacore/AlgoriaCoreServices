using AlgoriaCore.Application.QueriesAndCommands.SampleData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [Authorize]
    public class SampleDataController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<IEnumerable<WeatherForecast>> WeatherForecasts(string id)
        {
            // Ejemplo para obtener la sesión

            return await Mediator.Send(new GetSampleDataQuery { Id = id });
        }
    }
}
