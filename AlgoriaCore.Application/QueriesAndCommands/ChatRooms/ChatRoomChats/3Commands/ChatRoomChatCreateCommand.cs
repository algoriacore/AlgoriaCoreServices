using MediatR;
using System.Collections.Generic;

namespace AlgoriaCore.Application.QueriesAndCommands.ChatRooms.ChatRoomChats
{
    public class ChatRoomChatCreateCommand : IRequest<ChatRoomChatResponse>
    {
        public long ChatRoom { get; set; }
        public string Comment { get; set; }

        public List<long> TaggedUsers { get; set; }
        public List<ChatRoomChatFileCreateCommand> Files { get; set; }

        public ChatRoomChatCreateCommand()
        {
            TaggedUsers = new List<long>();
            Files = new List<ChatRoomChatFileCreateCommand>();
        }
    }

    public class ChatRoomChatFileCreateCommand
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Base64 { get; set; }
    }
}