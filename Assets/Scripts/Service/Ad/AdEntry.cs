using Service.Ad.Interfaces;

namespace Service.Ad
{
    public class AdEntry
    {
        public IAdAdapter Adapter;

        public string Zone;

        public int CurrentCount;
    }
}