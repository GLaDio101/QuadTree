using System.Collections.Generic;
using AssetBundles;

namespace Core.Manager.Bundle
{
    public class BundleInfoModel : IBundleInfoModel
    {
        private Dictionary<string, BundleInfoVo> _map;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _map = new Dictionary<string, BundleInfoVo>();
        }

        public void AddInfo(BundleInfoVo vo)
        {
            if (_map.ContainsKey(vo.key))
            {
//        Debug.LogWarning("Already have bundle info about " + vo.key);
                return;
            }

            _map.Add(vo.key, vo);
        }

        public string Root { get; set; }

        public string GetPath(string name)
        {
//      if (ServerInfo.Instance.Active.UseLocalBundles)
//      {
            return Utility.GetAssetBundlePath(name);
//      }
//
//      if (!_map.ContainsKey(name))
//      {
//        Debug.LogWarning("No bundle info about " + name);
//        return "";
//      }
//
//      return Root + _map[name].path;
        }
    }
}