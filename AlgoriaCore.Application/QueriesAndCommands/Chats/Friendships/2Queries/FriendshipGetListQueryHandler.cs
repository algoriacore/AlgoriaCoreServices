using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Chat;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Chat.ChatMessages;
using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using AlgoriaCore.Application.Managers.Chat.Friendships;
using AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._1Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._2Queries
{
    public class FriendshipGetListQueryHandler : BaseCoreClass, IRequestHandler<FriendshipGetListQuery, PagedResultDto<FriendshipListResponse>>
    {
        private readonly FriendshipManager _manager;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly ChatMessageManager _chatMessageManager;

        public FriendshipGetListQueryHandler(
            ICoreServices coreServices, 
            FriendshipManager manager, 
            IOnlineClientManager onlineClientManager,
            ChatMessageManager chatMessageManager) : base(coreServices)
        {
            _manager = manager;
            _onlineClientManager = onlineClientManager;
            _chatMessageManager = chatMessageManager;
        }

        public async Task<PagedResultDto<FriendshipListResponse>> Handle(FriendshipGetListQuery request, CancellationToken cancellationToken)
        {
            var ll = _manager.GetFriendshipList();

            var lista = new List<FriendshipListResponse>();

            ll.ForEach(dto => {

                var friend = new ChatUser(dto.FriendTenantId, dto.FriendUserId);
                var item = new FriendshipListResponse();

                item.Id = dto.Id.Value;
                item.FriendTenantId = dto.FriendTenantId;
                item.FriendUserId = dto.FriendUserId;
                item.FriendUserName = dto.FriendNickname;
                item.UnreadMessageCount = _chatMessageManager.GetUnreadMessagesCount(friend);
                item.IsOnline = _onlineClientManager.IsOnline(friend);
                item.State = dto.State;

                lista.Add(item);
            });

            return await Task.FromResult(new PagedResultDto<FriendshipListResponse>(ll.Count, lista));
        }
    }
}
