using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries
{
    public class RolGetForListActiveQueryHandler : BaseCoreClass, IRequestHandler<RolGetForListActiveQuery, List<RolForListActiveResponse>>
    {
        private readonly RolManager _rolManager;

        public RolGetForListActiveQueryHandler(ICoreServices coreServices, RolManager rolManager) : base(coreServices)
        {
            _rolManager = rolManager;
        }

        public async Task<List<RolForListActiveResponse>> Handle(RolGetForListActiveQuery request, CancellationToken cancellationToken)
        {
            var ll = await _rolManager.GetRolListActiveAsync();

            var lista = ll.Select(s => new RolForListActiveResponse
            {
                Id = s.Id.Value,
                Name = s.Name,
                DisplayName = s.DisplayName
            }).ToList();

            return lista;
        }
    }
}
