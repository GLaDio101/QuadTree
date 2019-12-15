using System;
using Core.Model.Vo;
using I2.Loc;

namespace Project.Model.Player
{
    [Serializable]
    public class SettingsVo : ISettingsVo
    {
        public float Volume { get; set; }

        public float Music { get; set; }

        public float Effects { get; set; }

        public bool ConnectedFb { get; set; }

        public short Quality { get; set; }

        public bool ShowRateUs { get; set; }

        public string Language
        {
            get { return LocalizationManager.CurrentLanguageCode; }
            set { }
        }

        public bool RemoveAds { get; set; }

        public int UserId { get; set; }

        public long DailyRewardTime { get; set; }

        public bool KeyboardOn { get; set; }

        public bool AdvancedVolume { get; set; }
    }
}