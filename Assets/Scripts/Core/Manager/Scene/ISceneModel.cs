using Core.Promise;

namespace Core.Manager.Scene
{
  public interface ISceneModel
  {
    IPromise<LoadData> LoadScene(string name, SceneLayer layer, bool reload = false);

    IPromise<LoadData> LoadScene(string name, string path, SceneLayer layer, bool reload = false);

    IPromise<SceneLayer> Clear(SceneLayer layer);

    void ClearLayers(SceneLayer[] layers);
  }
}