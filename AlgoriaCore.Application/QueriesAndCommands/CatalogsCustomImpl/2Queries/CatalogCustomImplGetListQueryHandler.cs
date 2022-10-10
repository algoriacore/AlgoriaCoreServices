using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl.Dto;
using MediatR;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetListQueryHandler : BaseCoreClass, IRequestHandler<CatalogCustomImplGetListQuery, PagedResultDto<Dictionary<string, object>>>
    {
        private readonly CatalogCustomImplManager _manager;

        public CatalogCustomImplGetListQueryHandler(ICoreServices coreServices, CatalogCustomImplManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<Dictionary<string, object>>> Handle(CatalogCustomImplGetListQuery request, CancellationToken cancellationToken)
        {
            await _manager.SetCatalogCustom(request.Catalog);

            CatalogCustomImplListFilterDto filterDto = new CatalogCustomImplListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            PagedResultDto<BsonDocument> pagedResultDto = await _manager.GetCatalogCustomImplListAsync(filterDto);

            return new PagedResultDto<Dictionary<string, object>>(pagedResultDto.TotalCount, pagedResultDto.Items.Select(p => p.ToDictionary()).ToList());
        }
    }
}
