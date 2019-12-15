using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Core.Editor.Release
{
  public class PublishSettings
  {
    public static string BuildPath = "Releases/";

    public static string BundleRootPath = "Assets/StreamingAssets";

    public static string BundlePath = "/AssetBundles";

    public static string SceneName = "Game";

    public static string HostSceneName = "Host";

    public static string BundleExtension = "";

    public static bool BuildBundles = false;

    public static string BuildOutputPath
    {
      get { return BuildPath + PlatformName + Path.DirectorySeparatorChar; }
    }

    public static string BundleOutputPath
    {
      get { return BundleRootPath + BundlePath; }
    }

    public static Dictionary<BuildTarget, string> ExtMap = new Dictionary<BuildTarget, string>
    {
      {BuildTarget.Android, ".apk"},
      {BuildTarget.iOS, ".ipa"},
      {BuildTarget.StandaloneWindows64, ".exe"},
      {BuildTarget.StandaloneLinux, ".x86"},
      {BuildTarget.StandaloneLinux64, ".x86_64"},
      {BuildTarget.StandaloneLinuxUniversal, ".universal"},
      {BuildTarget.StandaloneOSX, ".app"}
    };

    public static string PlatformName;

    public static BuildOptions BuildOption;
  }
}