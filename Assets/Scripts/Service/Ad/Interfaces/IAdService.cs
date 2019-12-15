using System.Collections.Generic;
using Service.Ad.Enums;
using Service.Ad.Vo;

namespace Service.Ad.Interfaces
{
	public interface IAdService
	{
        int CurrentRewardAmount { get; }

	    void Init(IList<AdEntry> list);

	    bool Show(string zone, AdShowType type);

	    void Hide(string zone);

	    void Load(AdServiceData adServiceData);

	    List<AdServiceVo> GetAdServiceList();

	}
}