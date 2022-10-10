using System;

namespace AlgoriaCore.Application.Managers.ChatRooms.Dto
{
    public class ChatRoomChatFileDto
    {
        public long? Id { get; set; }
        public long ChatRoomChat { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public Guid? File { get; set; }

        public byte[] Bytes { get; set; }
    }
}
