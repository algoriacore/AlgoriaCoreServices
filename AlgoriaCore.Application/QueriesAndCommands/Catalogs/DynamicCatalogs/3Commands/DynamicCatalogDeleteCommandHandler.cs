using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._3Commands
{
    public class DynamicCatalogDeleteCommandHandler : BaseCoreClass, IRequestHandler<DynamicCatalogDeleteCommand, long>
    {
        private readonly DynamicCatalogsManager _manager;

        public DynamicCatalogDeleteCommandHandler(ICoreServices coreServices,
            DynamicCatalogsManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(DynamicCatalogDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.DeleteRegistroAsync(request.Tabla, request.Id);

            return 0;
        }
    }
}
