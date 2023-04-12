using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries
{
    public class RoleGetListQuery : PageListByDto, IRequest<PagedResultDto<RoleForListResponse>>
    {
    }
}
