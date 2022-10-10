using AlgoriaCore.Application.Chat;
using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using AlgoriaCore.Application.Managers.Chat.Friendships.Dto;
using AlgoriaCore.Domain.Interfaces.Logger;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace AlgoriaCore.WebUI.Chat.SignalR
{
    public class SignalRChatCommunicator : IChatCommunicator
    {
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly ICoreLogger _coreLogger;

        public SignalRChatCommunicator(
            IHubContext<ChatHub> chatHub,
            ICoreLogger coreLogger
            )
        {
            _chatHub = chatHub;
            _coreLogger = coreLogger;
        }

        public void SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessageData message)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    return;
                }

                signalRClient.SendAsync("getChatMessage", message);
            }
        }

        public void SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, ChatUser user, bool isConnected)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }

                signalRClient.SendAsync("getUserConnectNotification", user, isConnected);
            }
        }

        public void SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, ChatFriendData friend, bool isOwnRequest)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    return;
                }

                signalRClient.SendAsync("getFriendshipRequest", friend, isOwnRequest);
            }
        }

        public void SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, ChatUser user, FriendshipState newState)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }
				
                signalRClient.SendAsync("getUserStateChange", user, newState);
            }
        }

        public void SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, ChatUser user)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);
                if (signalRClient == null)
                {
                    continue;
                }
				
                signalRClient.SendAsync("getAllUnreadMessagesOfUserRead", user);
            }
        }

        #region Métodos privados

        private IClientProxy GetSignalRClientOrNull(IOnlineClient client)
        {
            var signalRClient = _chatHub.Clients.Client(client.ConnectionId);
            if (signalRClient == null)
            {
                _coreLogger.LogDebug("Can not get chat user " + client.UserId + " from SignalR hub!");
                return null;
            }

            return signalRClient;
        }

        #endregion
    }
}
