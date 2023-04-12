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
    public class RoleGetForListActiveQueryHandler : BaseCoreClass, IRequestHandler<RoleGetForListActiveQuery, List<RoleForListActiveResponse>>
    {
        private readonly RoleManager _roleManager;

        public RoleGetForListActiveQueryHandler(ICoreServices coreServices, RoleManager rolManager) : base(coreServices)
        {
            _roleManager = rolManager;
        }

        public async Task<List<RoleForListActiveResponse>> Handle(RoleGetForListActiveQuery request, CancellationToken cancellationToken)
        {
            var ll = await _roleManager.GetRolListActiveAsync();

            var lista = ll.Select(s => new RoleForListActiveResponse
            {
                Id = s.Id.Value,
                Name = s.Name,
                DisplayName = s.DisplayName
            }).ToList();

            return lista;
        }
    }
}
