using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomDeleteCommandHandler : BaseCoreClass, IRequestHandler<CatalogCustomDeleteCommand, string>
    {
        private readonly CatalogCustomManager _manager;

        public CatalogCustomDeleteCommandHandler(ICoreServices coreServices, CatalogCustomManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<string> Handle(CatalogCustomDeleteCommand request, CancellationToken cancellationToken)
        {
            await _manager.DeleteCatalogCustomAsync(request.Id);

            return request.Id;
        }
    }
}
