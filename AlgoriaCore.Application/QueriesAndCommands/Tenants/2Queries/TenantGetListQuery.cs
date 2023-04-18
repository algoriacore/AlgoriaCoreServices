using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Tenants._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants
{
    public class TenantGetListQuery : PageListByDto, IRequest<PagedResultDto<TenantListResponse>>
    {
    }
}
