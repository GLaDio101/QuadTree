namespace Core.Model.Vo
{
    public interface ISettingsVo
    {
        bool AdvancedVolume { get; set; }

        float Volume { get; set; }

        float Music { get; set; }

        float Effects { get; set; }

        bool ConnectedFb { get; set; }

        short Quality { get; set; }

        bool ShowRateUs { get; set; }

        string Language { get; set; }

        bool RemoveAds { get; set; }

        int UserId { get; set; }

        long DailyRewardTime { get; set; }

        bool KeyboardOn { get; set; }
    }
}