using Core.Promise;

namespace Core.Manager.Bundle
{
  public interface IBundleModel
  {
    IPromise<BundleLoadData> LoadBundle(string name, bool load = false);

    IPromise<BundleLoadData> LoadBundle(string name, string path, bool load = false);

    void Clear(string name, bool clearAll = true);

    void ClearLayers(string[] names);

    BundleLoadData GetBundleByName(string name);
  }
}