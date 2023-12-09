using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Catalogos.CatalogosDinamicos;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Catalogs.DynamicCatalogs._2Queries
{
    public class DataByIdQueryHandler : BaseCoreClass, IRequestHandler<DataByIdQuery, Dictionary<string, object>>
    {
        private readonly DynamicCatalogsManager _manager;

        public DataByIdQueryHandler(ICoreServices coreServices, DynamicCatalogsManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<Dictionary<string, object>> Handle(DataByIdQuery request, CancellationToken cancellationToken)
        {
            return await _manager.GetById(request.Tabla, request.Id);
        }
    }
}
