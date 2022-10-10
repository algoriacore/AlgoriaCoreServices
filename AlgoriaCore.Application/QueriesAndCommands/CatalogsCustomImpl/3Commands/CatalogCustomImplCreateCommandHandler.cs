using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplCreateCommandHandler : BaseCoreClass, IRequestHandler<CatalogCustomImplCreateCommand, string>
    {
        private readonly CatalogCustomImplManager _manager;

        public CatalogCustomImplCreateCommandHandler(ICoreServices coreServices, CatalogCustomImplManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<string> Handle(CatalogCustomImplCreateCommand request, CancellationToken cancellationToken)
        {
            await _manager.SetCatalogCustom(request.Catalog);

            return await _manager.CreateCatalogCustomImplAsync(_manager.ParseToBsonDocument(request.Data));
        }
    }
}
