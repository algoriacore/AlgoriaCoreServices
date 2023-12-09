using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._3Commands
{
    public class DynamicCatalogUpdateCommandHandler : BaseCoreClass, IRequestHandler<DynamicCatalogUpdateCommand, long>
    {
        private readonly DynamicCatalogsManager _manager;

        public DynamicCatalogUpdateCommandHandler(ICoreServices coreServices,
            DynamicCatalogsManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(DynamicCatalogUpdateCommand request, CancellationToken cancellationToken)
        {
            await _manager.UpdateRegistroAsync(request.Tabla, request.Data);

            return 0;
        }
    }
}
