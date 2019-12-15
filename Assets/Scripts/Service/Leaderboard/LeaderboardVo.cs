using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Service.Leaderboard
{
    public class LeaderboardVo
    {
        public int Index;

        public string Username;

        public string Point;

        public Texture2D Image;

        public bool IsFriend;

        public UserState State;

        public DateTime Date;
    }
}