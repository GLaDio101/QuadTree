using System;
using Service.DailyReward;
using UnityEngine;

namespace Service.Achievements
{
    public class AcvievementVo
    {
        public string Id;

        public string Description;

        public string Title;

        public int Points;

        public bool IsHidden;

        public Texture2D Image;

        public DailyRewardVo DailyReward;

        public double Percentage;

        public DateTime Date;
    }
}