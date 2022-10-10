using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.SampleDateData;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class SampleDateDataController : BaseController
    {
        [HttpPost]
        public async Task<PagedResultDto<SampleDateDataForListResponse>> GetSampleDateDataList([FromBody]SampleDateDataGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}")]
        public async Task<SampleDateDataResponse> GetSampleDateData(int id)
        {
            return await Mediator.Send(new SampleDateDataGetByIdQuery { Id = id });
        }

        [HttpPost]
        public async Task<SampleDateDataForEditResponse> GetSampleDateDataForEdit(SampleDateDataGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<long> CreateSampleDateData([FromBody]SampleDateDataCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<long> UpdateSampleDateData([FromBody]SampleDateDataUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost("{id}")]
        public async Task<long> DeleteSampleDateData(int id)
        {
            return await Mediator.Send(new SampleDateDataDeleteCommand { Id = id });
        }

        [HttpPost]
        public async Task<DateTime> ConvertSampleDateData([FromBody]SampleDateDataConvertCommand dto)
        {
            return await Mediator.Send(dto);
        }
    }
}
