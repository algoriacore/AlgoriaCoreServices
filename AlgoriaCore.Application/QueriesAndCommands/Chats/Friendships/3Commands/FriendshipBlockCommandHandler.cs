using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Chat;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using AlgoriaCore.Application.Managers.Chat.Friendships;
using AlgoriaCore.Application.Managers.Chat.Friendships.Dto;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.Friendships._3Commands
{
    public class FriendshipBlockCommandHandler : BaseCoreClass, IRequestHandler<FriendshipBlockCommand, long>
    {
        private readonly FriendshipManager _manager;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly IChatCommunicator _chatCommunicator;

        public FriendshipBlockCommandHandler(
            ICoreServices coreServices,
            FriendshipManager manager,
            IOnlineClientManager onlineClientManager,
            IChatCommunicator chatCommunicator
            ) : base(coreServices)
        {
            _manager = manager;
            _onlineClientManager = onlineClientManager;
            _chatCommunicator = chatCommunicator;
        }

        public async Task<long> Handle(FriendshipBlockCommand request, CancellationToken cancellationToken)
        {
            var resp = _manager.BlockFriendship(request.FriendTenantId, request.FriendUserId);

            var senderChatUser = new ChatUser(SessionContext.TenantId, SessionContext.UserId.Value);
            var friendChatUser = new ChatUser(request.FriendTenantId, request.FriendUserId);

            var clients = _onlineClientManager.GetAllByUserId(senderChatUser);

            if (clients.Any())
            {
                _chatCommunicator.SendUserStateChangeToClients(clients, friendChatUser, FriendshipState.Blocked);
            }

            return await Task.FromResult(resp);
        }
    }
}
