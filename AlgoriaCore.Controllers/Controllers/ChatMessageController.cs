using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Chats.ChatMessages._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Chats.ChatMessages._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Chats.ChatMessages._3Commands;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class ChatMessageController : BaseController
    {
        [HttpPost]
        public async Task<PagedResultDto<ChatMessageListResponse>> GetChatMessageList([FromBody]ChatMessageGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<long> MarkAllUnreadMessagesOfUserAsRead([FromBody]ChatMessageMarkReadCommand query)
        {
            return await Mediator.Send(query);
        }
    }
}