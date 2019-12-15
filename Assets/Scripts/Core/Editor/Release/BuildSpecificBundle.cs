using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Release
{
    public enum Collection
    {
        Minimum,
        Cars,
        Environments,
        Level1To5,
        AllLevels,
        Core
    }

    public class BuildSpecificBundle : ScriptableWizard
    {
        public string BundleName;

        public bool BuildSingle;

        public Collection BundleCollection;

        private static Dictionary<Collection, List<string>> _list;

        [MenuItem("Release/Build Specific Bundle #&%b", false, 201)]
        [UsedImplicitly]
        private static void SelectBundleName()
        {
            ScriptableWizard.DisplayWizard("Select Bundle to Build", typeof(BuildSpecificBundle), "Build");
        }

        [UsedImplicitly]
        private void OnEnable()
        {
            minSize = maxSize = new Vector2(400, 160);
        }

        [UsedImplicitly]
        private void OnWizardCreate()
        {
            Publish.CleanCache();

            _list = new Dictionary<Collection, List<string>>
            {
                {Collection.Minimum, new List<string>(){ "items", "shaders","ram", "level1", "desert", "garage" } },
                {Collection.Environments, new List<string>(){ "desert"} },
                {Collection.Core, new List<string>(){ "garage","items","shaders"} },
                {Collection.Level1To5, new List<string>(){ "level1", "level2", "level3", "level4", "level5" } },
                {Collection.Cars, new List<string>(){ "ram","rover","monster","wrangler","hummer","raptor" } }
            };

            var bundleNameList = _list[BundleCollection];
            if (BuildSingle)
            {
                bundleNameList = new List<string>() { BundleName };
            }

            PublishSettings.PlatformName = Publish.GetPlatformForAssetBundles();

            // clear previous files
            var outputPath = Path.Combine(PublishSettings.BundlePath, PublishSettings.PlatformName);

            BuildPipeline.BuildAssetBundles(outputPath, GetBundleData(bundleNameList), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

            AssetDatabase.Refresh();
        }

        private static AssetBundleBuild[] GetBundleData(IList<string> bundleNameList)
        {
            AssetBundleBuild[] list = new AssetBundleBuild[bundleNameList.Count];
            for (int i = 0; i < bundleNameList.Count; i++)
            {
                list[i] = new AssetBundleBuild()
                {
                    assetBundleName = bundleNameList[i],
                    assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(bundleNameList[i])
                };
            }

            return list;
        }
    }
}