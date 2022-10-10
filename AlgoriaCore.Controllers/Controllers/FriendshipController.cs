using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._3Commands;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class FriendshipController : BaseController
    {
        [HttpPost]
        public async Task<PagedResultDto<FriendshipListResponse>> GetFriendshipList([FromBody]FriendshipGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<long> CreateFriendship([FromBody]FriendshipCreateCommand query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<long> BlockFriendship([FromBody]FriendshipBlockCommand query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<long> UnblockFriendship([FromBody]FriendshipUnblockCommand query)
        {
            return await Mediator.Send(query);
        }
    }
}