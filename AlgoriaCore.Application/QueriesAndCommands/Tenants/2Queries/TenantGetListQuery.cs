using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Tenants._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class TenantGetListQuery : PageListByDto, IRequest<PagedResultDto<TenantListResponse>>
    {
    }
}
