using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using AlgoriaCore.Application.Managers.Chat.Friendships.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Chat
{
    public interface IChatCommunicator
    {
        void SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessageData message);

        void SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, ChatFriendData friend, bool isOwnRequest);

        void SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, ChatUser user, bool isConnected);

        void SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, ChatUser user, FriendshipState newState);

        void SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, ChatUser user);
    }
}
