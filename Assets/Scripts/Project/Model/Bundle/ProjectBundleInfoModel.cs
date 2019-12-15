using AssetBundles;
using Core.Manager.Bundle;
using UnityEditor;
using UnityEngine;

namespace Project.Model.Bundle
{
  public class ProjectBundleInfoModel : BundleInfoModel, IProjectBundleInfoModel
  {
    private string platform;

    [PostConstruct]
    public new void OnPostConstruct()
    {
#if UNITY_EDITOR
      platform = Utility.GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
      platform = Utility.GetPlatformForAssetBundles(Application.platform);
#endif
    }

    private void AddBase(string type, string key, int version)
    {
      AddInfo(new BundleInfoVo
      {
        path = "uss/static/" + type + "/" + key + "." + platform + ".v" + version + ".unity3d",
        version = version,
        key = key
      });
    }

    public void AddCloth(string key, int version)
    {
      AddBase("clothes", key, version);
    }

    public void AddFurniture(string key, int version)
    {
      AddBase("furniture", key, version);
    }

    public void AddItem(string key, int version)
    {
      AddBase("items", key, version);
    }

    public void AddRoom(string key, int version)
    {
      AddBase("rooms", key, version);
    }
  }
}