using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Service.Friends
{
    public class FriendVo
    {
        public string Username;

        public UserState State;

        public Texture2D Image;

        public string Id;

        public bool IsFriend;
    }
}