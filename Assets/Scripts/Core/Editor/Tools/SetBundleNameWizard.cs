using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class SetBundleNameWizard : ScriptableWizard
    {
        public string NewName;

        [MenuItem("Tools/Asset Bundle/Set NameLabel")]
        [UsedImplicitly]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Set Bundle NameLabel", typeof(SetBundleNameWizard), "Apply");
        }

        [UsedImplicitly]
        private void OnWizardCreate()
        {
            if (Selection.objects.Length == 0)
            {
                Debug.LogWarning("Select object from project view.");
                return;
            }
            foreach (var obj in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                AssetImporter importer = AssetImporter.GetAtPath(path);
                if (importer)
                    importer.assetBundleName = NewName;
                else
                    Debug.LogWarning(path + " not found!");
            }

        }

        [MenuItem("Tools/Asset Bundle/Clear NameLabel")]
        [UsedImplicitly]
        private static void Clear()
        {
            if (Selection.objects.Length == 0)
            {
                Debug.LogWarning("Select object from project PanelView.");
                return;
            }
            foreach (var obj in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                AssetImporter importer = AssetImporter.GetAtPath(path);
                if (importer)
                    importer.assetBundleName = String.Empty;
                else
                    Debug.LogWarning(path + " not found!");
            }
        }

        [MenuItem("Tools/Asset Bundle/Set From Filename")]
        [UsedImplicitly]
        private static void FromFileName()
        {
            if (Selection.objects.Length == 0)
            {
                Debug.LogWarning("Select object from project PanelView.");
                return;
            }
            foreach (var obj in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                var name = path.Substring(path.LastIndexOf("/", StringComparison.Ordinal) + 1);
                var ext = path.Substring(path.LastIndexOf(".", StringComparison.Ordinal) + 1);
                name = name.Replace("." + ext, "");
                AssetImporter importer = AssetImporter.GetAtPath(path);
                if (importer)
                    importer.assetBundleName = name.ToLower();
                else
                    Debug.LogWarning(path + " not found!");
            }
        }

        [MenuItem("Tools/Asset Bundle/Remove Unused Names")]
        [UsedImplicitly]
        private static void RemoveUnusedAssetBundleNames()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }
    }
}