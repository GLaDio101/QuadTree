using System;
using System.Collections.Generic;

namespace Core.Manager.Bundle
{
  [Serializable]
  public class BundleInfoVo
  {
    public string key;

    public int version;

    public string path;
  }

  [Serializable]
  public class BundleInfoResponseVo
  {
    public List<BundleInfoVo> list;

    public string root;
  }

  [Serializable]
  public class BundleInfoRequestVo
  {
    public string platform;

    public string type;
  }
}