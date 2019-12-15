using System.Collections.Generic;

namespace Service.Friends
{
    public interface IFriendsService
    {
        List<FriendVo> List { get; }

        void Load();
    }
}