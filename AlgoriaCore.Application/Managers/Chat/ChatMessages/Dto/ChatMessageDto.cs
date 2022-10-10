using System;

namespace AlgoriaCore.Application.Managers.Chat.ChatMessages.Dto
{
    public class ChatMessageDto
    {
        public long? Id { get; set; }

        public long UserId { get; set; }

        public int? TenantId { get; set; }

        public long TargetUserId { get; set; }

        public int? TargetTenantId { get; set; }

        public string Message { get; set; }

        public DateTime CreationTime { get; set; }

        public ChatSide Side { get; set; } // Sender = 1, Receiver = 2

        public ChatMessageReadState ReadState { get; set; } // Unread = 1, Read = 2

        public ChatMessageDto()
        {

        }

        public ChatMessageDto(
            ChatUser user,
            ChatUser targetUser,
            ChatSide side,
            string message,
            ChatMessageReadState readState)
        {
            UserId = user.UserId;
            TenantId = user.TenantId;
            TargetUserId = targetUser.UserId;
            TargetTenantId = targetUser.TenantId;
            Message = message;
            Side = side;
            ReadState = readState;

            CreationTime = DateTime.Now;
        }
    }
}
