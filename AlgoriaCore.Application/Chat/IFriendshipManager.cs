using AlgoriaCore.Application.Managers.Chat.Friendships.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Chat
{
    public interface IFriendshipManager
    {
        FriendshipDto GetFriendshipOrNull(FriendshipBaseDto dto);

        List<FriendshipDto> GetFriendshipList(int? tenantId, long userId);
    }
}
