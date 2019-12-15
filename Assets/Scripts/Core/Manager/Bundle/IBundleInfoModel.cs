namespace Core.Manager.Bundle
{
  public interface IBundleInfoModel
  {
    void AddInfo(BundleInfoVo vo);

    string Root { get; set; }

    string GetPath(string name);
  }
}