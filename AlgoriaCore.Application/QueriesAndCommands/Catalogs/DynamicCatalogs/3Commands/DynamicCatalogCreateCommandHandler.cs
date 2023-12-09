using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._3Commands
{
    public class DynamicCatalogCreateCommandHandler : BaseCoreClass, IRequestHandler<DynamicCatalogCreateCommand, long>
    {
        private readonly DynamicCatalogsManager _manager;

        public DynamicCatalogCreateCommandHandler(ICoreServices coreServices,
            DynamicCatalogsManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(DynamicCatalogCreateCommand request, CancellationToken cancellationToken)
        {
            await _manager.CreateRegistroAsync(request.Tabla, request.Data);

            return 0;
        }
    }
}
