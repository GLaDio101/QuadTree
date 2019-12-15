using System;

namespace Service.DailyReward
{
    [Serializable]
    public class RewardUserData
    {
        public int LastSessionTime { get; set; }

        public int SuccessiveSessionCount { get; set; }
    }
}