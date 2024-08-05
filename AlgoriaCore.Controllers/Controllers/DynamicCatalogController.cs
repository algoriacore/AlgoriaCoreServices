using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._3Commands;
using AlgoriaCore.WebUI.Controllers;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class DynamicCatalogController : BaseController
    {
        [HttpPost]
        public async Task<List<ComboboxItemDto>> GetForCombobox(DynamicCatalogGetComboboxQuery request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        public async Task<DynamicCatalogViewInfoResponse> GetDynamicCatalogVistaDefinitionsList(DynamicCatalogGetVistaInfoQuery request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        public async Task<DynamicCatalogFormInfoResponse> GetDynamicCatalogFormDefinitionsList(DynamicCatalogGetFormInfoQuery request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        public async Task<List<Dictionary<string, object>>> GetList(DataListQuery request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        public async Task<Dictionary<string, object>> GetById(DataByIdQuery request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        public async Task<long> Create(DynamicCatalogCreateCommand request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        public async Task<long> Update(DynamicCatalogUpdateCommand request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost]
        public async Task<long> Delete(DynamicCatalogDeleteCommand request)
        {
            return await Mediator.Send(request);
        }
    }
}
