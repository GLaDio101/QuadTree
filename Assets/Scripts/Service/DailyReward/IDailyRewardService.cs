namespace Service.DailyReward
{
    public interface IDailyRewardService
    {
        RewardUserData UserData { get; }

        bool IsRewardReady { get; }

        bool IsRewardMissed { get; }

        int CurrentTime { get; }

        int RemainingTime { get; }

        DailyRewardVo GetReward();

        void Init(RewardUserData userData);

        void AddReward(DailyRewardVo dailyRewardVo);
    }
}