using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats;
using AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRooms;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class ChatRoomController : BaseController
    {
        #region CHAT ROOMS

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_ChatRooms)]
        [HttpPost]
        public async Task<PagedResultDto<ChatRoomForListResponse>> GetChatRoomList([FromBody]ChatRoomGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{id}")]
        public async Task<ChatRoomResponse> GetChatRoom(long id)
        {
            return await Mediator.Send(new ChatRoomGetByIdQuery { Id = id });
        }

        [HttpPost]
        public async Task<ChatRoomResponse> GetChatRoomByChatRoomId(string chatRoomId)
        {
            return await Mediator.Send(new ChatRoomGetByChatRoomIdQuery() { ChatRoomId = chatRoomId });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_ChatRooms_Edit)]
        [HttpPost]
        public async Task<ChatRoomForEditResponse> GetChatRoomForEdit(ChatRoomGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<ChatRoomResponse> GetOrCreateChatRoom([FromBody]ChatRoomGetOrCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<long> CreateChatRoom([FromBody]ChatRoomCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_ChatRooms_Edit)]
        [HttpPost]
        public async Task<long> UpdateChatRoom([FromBody]ChatRoomUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        #endregion

        #region MESSAGES

        [HttpPost]
        public async Task<PagedResultDto<ChatRoomChatForListResponse>> GetChatRoomChatList([FromBody]ChatRoomChatGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ChatRoomChatResponse> CreateChatRoomChat([FromBody]ChatRoomChatCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        #endregion
    }
}
