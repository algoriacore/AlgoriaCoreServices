using AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto;
using System;

namespace AlgoriaCore.Application.QueriesAndCommands.Chats.ChatMessages._1Model
{
    public class ChatMessageListResponse
    {
        public long Id { get; set; }

        public long TargetUserId { get; set; }

        public int? TargetTenantId { get; set; }

        public string Message { get; set; }

        public DateTime CreationTime { get; set; }

        public ChatSide Side { get; set; } // Sender = 1, Receiver = 2

        public ChatMessageReadState ReadState { get; set; } // Unread = 1, Read = 2
    }
}
