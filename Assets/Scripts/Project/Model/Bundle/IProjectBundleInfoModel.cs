namespace Project.Model.Bundle
{
  public interface IProjectBundleInfoModel
  {
    void AddCloth(string key, int version);

    void AddFurniture(string key, int version);

    void AddRoom(string key, int version);

    void AddItem(string key, int version);
  }
}