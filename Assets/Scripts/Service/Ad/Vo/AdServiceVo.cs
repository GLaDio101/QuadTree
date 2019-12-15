using System;
using Service.Ad.Enums;

namespace Service.Ad.Vo
{
    [Serializable]
    public class AdServiceVo
    {
        public AdServiceType AdServiceType;

        public int Duration;

        public int RewardAmount;

        public int SkipButtonDuration;

        public string ImpressionLink;

        public string ClickLink;

        public string VideoId;

        public string IosZoneId;

        public string AndroidZoneId;

        public string AndroidGameId;

        public string IosGameId;

        public string ZoneId;

        public string Zone;
    }
}