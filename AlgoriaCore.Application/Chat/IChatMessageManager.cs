using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using System;

namespace AlgoriaCore.Application.Chat
{
    public interface IChatMessageManager
    {
        void SendMessage(ChatUser sender, ChatUser receiver, string message, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId);
    }
}
