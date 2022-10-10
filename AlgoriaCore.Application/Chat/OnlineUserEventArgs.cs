using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;

namespace AlgoriaCore.Application.Chat
{
    public class OnlineUserEventArgs : OnlineClientEventArgs
    {
        public ChatUser User { get; }

        public OnlineUserEventArgs(ChatUser user, IOnlineClient client) : base(client)
        {
            this.User = user;
        }
    }
}
