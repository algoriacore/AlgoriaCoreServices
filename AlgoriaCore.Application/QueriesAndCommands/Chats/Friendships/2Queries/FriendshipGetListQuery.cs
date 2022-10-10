using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._2Queries
{
    public class FriendshipGetListQuery : IRequest<PagedResultDto<FriendshipListResponse>>
    {
    }
}
