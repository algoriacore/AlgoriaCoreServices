using System;

namespace AlgoriaCore.Application.Chat
{
    public class OnlineClientEventArgs : EventArgs
    {
        public IOnlineClient Client { get; }

        public OnlineClientEventArgs(IOnlineClient client)
        {
            this.Client = client;
        }
    }

}
