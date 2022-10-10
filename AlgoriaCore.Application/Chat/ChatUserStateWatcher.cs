using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using AlgoriaCore.Domain.Session;
using System.Linq;

namespace AlgoriaCore.Application.Chat
{
    public class ChatUserStateWatcher : IChatUserStateWatcher
    {
        private readonly IChatCommunicator _chatCommunicator;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly IFriendshipManager _friendshipManager;
        private IAppSession _session;

        public ChatUserStateWatcher(
            IAppSession session,
            IChatCommunicator chatCommunicator,
            IOnlineClientManager onlineClientManager,
            IFriendshipManager friendshipManager
            )
        {
            _session = session;
            _chatCommunicator = chatCommunicator;
            _onlineClientManager = onlineClientManager;
            _friendshipManager = friendshipManager;
        }

        public void Initialize()
        {
            _onlineClientManager.UserConnected += OnlineClientManager_UserConnected;
            _onlineClientManager.UserDisconnected += OnlineClientManager_UserDisconnected;
        }

        private void OnlineClientManager_UserConnected(object sender, OnlineUserEventArgs e)
        {
            NotifyUserConnectionStateChange(e.User, true);
        }

        private void OnlineClientManager_UserDisconnected(object sender, OnlineUserEventArgs e)
        {
            NotifyUserConnectionStateChange(e.User, false);
        }

        private void NotifyUserConnectionStateChange(ChatUser user, bool isConnected)
        {
            if (_session == null)
            {
                _session = new SessionContext();
            }

            _session.TenantId = user.TenantId;
            _session.UserId = user.UserId;

            var ll = _friendshipManager.GetFriendshipList(user.TenantId, user.UserId);

            _session = null;

            foreach (var friend in ll)
            {
                System.Diagnostics.Debug.WriteLine("Buscando clientes signalR del Tenant: " + friend.FriendTenantId + " Usuario: " + friend.FriendUserId);
                var friendUserClients = _onlineClientManager.GetAllByUserId(new ChatUser(friend.FriendTenantId, friend.FriendUserId));
                System.Diagnostics.Debug.WriteLine("Buscando clientes signalR del Tenant: " + friend.FriendTenantId + " Usuario: " + friend.FriendUserId + " Encontrados: " + friendUserClients.Count);

                if (!friendUserClients.Any())
                {
                    continue;
                }

                System.Diagnostics.Debug.WriteLine("Informando clientes signalR del Tenant: " + friend.TenantId + " Usuario: " + friend.UserId);
                _chatCommunicator.SendUserConnectionChangeToClients(friendUserClients, user, isConnected);
            }
        }
    }
}
