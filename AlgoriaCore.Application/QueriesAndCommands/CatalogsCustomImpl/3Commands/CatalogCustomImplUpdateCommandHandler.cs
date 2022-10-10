using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplUpdateCommandHandler : BaseCoreClass, IRequestHandler<CatalogCustomImplUpdateCommand, string>
    {
        private readonly CatalogCustomImplManager _manager;

        public CatalogCustomImplUpdateCommandHandler(ICoreServices coreServices, CatalogCustomImplManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<string> Handle(CatalogCustomImplUpdateCommand request, CancellationToken cancellationToken)
        {
            await _manager.SetCatalogCustom(request.Catalog);

            BsonDocument dto = _manager.ParseToBsonDocument(request.Data);

            dto.Add("_id", new ObjectId(request.Id));

            await _manager.UpdateCatalogCustomImplAsync(dto);

            return request.Id;
        }
    }
}
