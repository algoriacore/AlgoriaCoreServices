using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetByIdQueryHandler : BaseCoreClass, IRequestHandler<CatalogCustomImplGetByIdQuery, CatalogCustomImplResponse>
    {
        private readonly CatalogCustomImplManager _manager;

        public CatalogCustomImplGetByIdQueryHandler(ICoreServices coreServices, CatalogCustomImplManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<CatalogCustomImplResponse> Handle(CatalogCustomImplGetByIdQuery request, CancellationToken cancellationToken)
        {
            await _manager.SetCatalogCustom(request.Catalog);

            BsonDocument dto = await _manager.GetCatalogCustomImplAsync(request.Id);

            return new CatalogCustomImplResponse()
            {
                Id = request.Id,
                Data = _manager.ParseToDictionary(dto)
            };
        }
    }
}
