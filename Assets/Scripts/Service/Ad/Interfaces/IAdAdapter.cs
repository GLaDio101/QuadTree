namespace Service.Ad.Interfaces
{
    public interface IAdAdapter
    {
        void Init(IAdServiceInternal adservice, string zone);

        void Load();

        bool Show(bool time);

        void Hide();

        bool IsLoaded { get; }

        bool IsTimeUp { get; }

        int Duration { get; set; }

        int RemainingTime { get; }

        int Count { get; set; }

        int RewardAmount { get; set; }

        string ZoneId { get; set; }

        void Destroy();
    }
}