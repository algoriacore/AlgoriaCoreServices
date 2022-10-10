using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Users._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserGetListQuery : PageListByDto, IRequest<PagedResultDto<UserListResponse>>
    {
        public int? Tenant { get; set; }
    }
}
