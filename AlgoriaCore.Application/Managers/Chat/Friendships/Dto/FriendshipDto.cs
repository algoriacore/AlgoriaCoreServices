using System;

namespace AlgoriaCore.Application.Managers.Chat.Friendships.Dto
{
    public class FriendshipDto : FriendshipBaseDto
    {
        public long? Id { get; set; }        
        public DateTime? CreationTime { get; set; }
        public string FriendNickname { get; set; }
        public FriendshipState State { get; set; }
    }
}
