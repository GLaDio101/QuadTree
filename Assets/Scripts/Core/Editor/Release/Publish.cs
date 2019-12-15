using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Core.Editor.Release
{
  public class Publish : UnityEditor.Editor
  {
    public static void CleanCache()
    {
      Debug.LogWarning(Caching.ClearCache() ? "Successfully cleaned all caches." : "Cache was in use.");
    }

    public static void Release()
    {
      Build(EditorUserBuildSettings.activeBuildTarget, PublishSettings.SceneName);
    }

    public static void BuildBundles()
    {
      BuildBundles(EditorUserBuildSettings.activeBuildTarget);
    }

    public static void BuildBundles(BuildTarget target)
    {
      CleanCache();

      var outputPath = Path.Combine(PublishSettings.BundleOutputPath, GetPlatformForAssetBundles(target));

      CreateDir(outputPath);

      BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, target);
    }

    public static void Run(BuildTarget target, string namePostfix = "")
    {
      PublishSettings.PlatformName = GetPlatformForAssetBundles(target);

      var outputName = GetFullAppName(target, namePostfix);
      Debug.Log(outputName);
      var myProcess = new Process {StartInfo = {FileName = outputName}};
//      var myProcess = new Process {StartInfo = {FileName = outputName,Arguments = "-batchmode"}};
      myProcess.Start();
    }

    public static void Build(BuildTarget target, string sceneName, string namePostfix = "")
    {
      CleanCache();

      if (PublishSettings.BuildBundles)
        BuildBundles(target);

      PublishSettings.PlatformName = GetPlatformForAssetBundles(target);

      CreateDir(PublishSettings.BuildOutputPath);

      var folderName = GetAppName(namePostfix);

      CreateDir(PublishSettings.BuildOutputPath + folderName);

      var outputName = GetFullAppName(target, namePostfix);

      var levels = GetBuildLevels(sceneName);

      if (PublishSettings.BuildBundles)
        PrepareBundleFolder(target);

//      Debug.Log("Build Summary ");
//      Debug.Log("---------------------");
//      Debug.Log("Output Name :" + outputName);//"WebGL-Dist"
//      Debug.Log("Level :" + string.Join(",", levels));
//      Debug.Log("Target :" + target);
//      Debug.Log("Options :" + PublishSettings.BuildOption);
//      Debug.Log("---------------------");

      if(target == BuildTarget.WebGL)
        BuildPipeline.BuildPlayer(levels, GetAppName("-webgl"), target, PublishSettings.BuildOption);
      else
        BuildPipeline.BuildPlayer(levels, outputName, target, PublishSettings.BuildOption);

      if (PublishSettings.BuildBundles)
        RevertBundleFolder(target);
    }

    private static void PrepareBundleFolder(BuildTarget target)
    {
      var outputPath = Path.Combine(PublishSettings.BundleOutputPath, GetPlatformForAssetBundles(target));
      var tempRoot = PublishSettings.BundleRootPath + "Temp";
      var tempBundlePath = Path.Combine(tempRoot + PublishSettings.BundlePath, GetPlatformForAssetBundles(target));


      AssetDatabase.MoveAsset(PublishSettings.BundleRootPath, tempRoot);
      AssetDatabase.CreateFolder("Assets", "StreamingAssets");
      AssetDatabase.CreateFolder("Assets/StreamingAssets", "AssetBundles");
      AssetDatabase.MoveAsset(tempBundlePath, outputPath);
    }

    private static void RevertBundleFolder(BuildTarget target)
    {
      var outputPath = Path.Combine(PublishSettings.BundleOutputPath, GetPlatformForAssetBundles(target));
      var tempRoot = PublishSettings.BundleRootPath + "Temp";
      var tempBundlePath = Path.Combine(tempRoot + PublishSettings.BundlePath, GetPlatformForAssetBundles(target));

      AssetDatabase.MoveAsset(outputPath, tempBundlePath);
      AssetDatabase.DeleteAsset(PublishSettings.BundleRootPath);
      AssetDatabase.MoveAsset(tempRoot, PublishSettings.BundleRootPath);
    }

    public static void CreateDir(string path)
    {
      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
        Debug.Log(path + " folder created");
      }
    }

    private static string[] GetBuildLevels(string levelName)
    {
      var levels = new string[] {"Assets/Scenes/" + levelName + ".unity"};
      return levels;
    }

    private static string GetFullAppName(BuildTarget target, string extra = "")
    {
      var outputName = PublishSettings.BuildOutputPath + GetAppName(extra) + "/";
      if (PublishSettings.ExtMap.ContainsKey(target))
        outputName += GetAppName(extra) + PublishSettings.ExtMap[target];
      Debug.Log("outputName: " + outputName);
      return outputName;
    }

    public static string GetAppName(string extra = "")
    {
      var folderName = PlayerSettings.productName.ToLower().Replace(" ", "-").Replace("ö", "o");
      folderName += extra + "-v" + PlayerSettings.bundleVersion;
      Debug.Log("folderName: " + folderName);
      return folderName;
    }

    public static void AddExtension()
    {
      string[] files = Directory.GetFiles(Path.Combine(PublishSettings.BundlePath, GetPlatformForAssetBundles()));

      foreach (var a in files)
      {
        if (!a.Contains("."))
        {
          FileUtil.DeleteFileOrDirectory(a + PublishSettings.BundleExtension);
          FileUtil.MoveFileOrDirectory(a, a + PublishSettings.BundleExtension);
        }
      }
    }

    public static string GetPlatformForAssetBundles()
    {
      return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
    }

    public static string GetPlatformForAssetBundles(BuildTarget target)
    {
      switch (target)
      {
        case BuildTarget.Android:
          return "Android";
        case BuildTarget.iOS:
          return "iOS";
        case BuildTarget.WebGL:
          return "WebGL";
        case BuildTarget.StandaloneWindows:
        case BuildTarget.StandaloneWindows64:
          return "Windows";
        case BuildTarget.StandaloneOSX:
          return "OSX";
        case BuildTarget.StandaloneLinux:
        case BuildTarget.StandaloneLinux64:
        case BuildTarget.StandaloneLinuxUniversal:
          return "Linux";
        // AddTrooper more build targets for your own.
        // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
        default:

          return null;
      }
    }
  }
}