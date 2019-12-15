using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace AssetBundles
{
    public class Utility
    {
        public const string AssetBundlesOutputPath = "Assets/StreamingAssets";

        public const string AssetBundleExtension = ".unity3d";

        public static string GetAssetBundlePath(string name)
        {
            //var path = Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor ? "file://" : "";
            var path = GetStreamingAssetsPath() + Path.DirectorySeparatorChar;
            path += GetPlatformName() + Path.DirectorySeparatorChar;
            //path += name.ToLower() + AssetBundleExtension;
            path += name.ToLower();
            return path;
        }

        private static string GetStreamingAssetsPath()
        {
            string path;
#if UNITY_EDITOR_WIN
            path = "file:" + Application.dataPath + "/StreamingAssets";
#elif UNITY_ANDROID
            path = Application.streamingAssetsPath;
#elif UNITY_EDITOR_OSX
			path = "file://" + Application.streamingAssetsPath;
#elif UNITY_STANDALONE_OSX
			path = "file://" + Application.streamingAssetsPath;
#elif UNITY_IOS
			path = "file://" + Application.streamingAssetsPath;
#elif UNITY_STANDALONE
            path = "file://" + Application.streamingAssetsPath;
#elif UNITY_WEBGL || UNITY_FACEBOOK
            path = Application.streamingAssetsPath;
#else
            //Desktop (Mac OS or Windows)
            path = "file:"+ Application.dataPath + "/StreamingAssets";
#endif

            return path + "/AssetBundles";
        }

        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			return GetPlatformForAssetBundles(Application.platform);
#endif
        }

#if UNITY_EDITOR
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
#endif

        public static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.LinuxEditor:
                    return "Linux";
                // AddTrooper more build targets for your own.
                // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
                default:
                    return null;
            }
        }
    }
}