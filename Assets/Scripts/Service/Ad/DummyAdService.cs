using System.Collections.Generic;
using Service.Ad.Enums;
using Service.Ad.Interfaces;
using Service.Ad.Vo;

namespace Service.Ad
{
    public class DummyAdService:IAdService
    {
        public void Init(IList<AdEntry> list)
        {
        }

        public bool Show(string zone, AdShowType type)
        {
            return false;
        }

        public void Hide(string zone)
        {
        }

        public int CurrentRewardAmount { get; set; }
        public void Load(AdServiceData adServiceData)
        {
            throw new System.NotImplementedException();
        }

        public List<AdServiceVo> GetAdServiceList()
        {
            throw new System.NotImplementedException();
        }
    }
}