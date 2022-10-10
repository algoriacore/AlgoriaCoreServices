using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplDeleteCommandHandler : BaseCoreClass, IRequestHandler<CatalogCustomImplDeleteCommand, string>
    {
        private readonly CatalogCustomImplManager _manager;

        public CatalogCustomImplDeleteCommandHandler(ICoreServices coreServices, CatalogCustomImplManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<string> Handle(CatalogCustomImplDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.SetCatalogCustom(request.Catalog);

            await _manager.DeleteCatalogCustomImplAsync(request.Id);

            return request.Id;
        }
    }
}
