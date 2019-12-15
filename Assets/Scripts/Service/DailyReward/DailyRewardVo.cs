using System;

namespace Service.DailyReward
{
    [Serializable]
    public class DailyRewardVo
    {
        public string Type { get; set; }

        public int Amount { get; set; } 
    }
}