using System;

namespace Service.DailyReward
{
    [Serializable]
    public class RewardVo
    {
        public string Type { get; set; }

        public int Amount { get; set; } 
    }
}