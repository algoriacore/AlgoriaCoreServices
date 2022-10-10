using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Application.QueriesAndCommands.Tenants._1Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants._2Queries
{
    public class TenantGetListCompleterQueryHandler : IRequestHandler<TenantGetListCompleterQuery, List<TenantListResponse>>
    {
        private readonly TenantManager _tenantManager;        

        public TenantGetListCompleterQueryHandler(TenantManager tenantManager)
        {
            _tenantManager = tenantManager;
        }

        public async Task<List<TenantListResponse>> Handle(TenantGetListCompleterQuery request, CancellationToken cancellationToken)
        {
            var ll = await _tenantManager.GetTenantsListCompleterAsync(request.Filter);

            var list = new List<TenantListResponse>();

            foreach (var item in ll)
            {
                list.Add(new TenantListResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                    TenancyName = item.TenancyName,
                    LargeName = item.LargeName
                });
            }

            return list;
        }
    }
}
