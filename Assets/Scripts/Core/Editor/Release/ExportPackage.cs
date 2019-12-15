using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Release
{
    public class ExportPackage : ScriptableWizard
    {
        private const string ExcludeLabel = "Exclude";

        public string[] ExcludeList = new string[1] { ExcludeLabel };

        [MenuItem("Tools/Export Package ", false, 202)]
        [UsedImplicitly]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Export Package", typeof(ExportPackage), "Export");
        }

        [UsedImplicitly]
        private void OnEnable()
        {
            minSize = maxSize = new Vector2(400, 160);
        }

        [UsedImplicitly]
        private void OnWizardCreate()
        {
            var assetsPaths = AssetDatabase.GetAllAssetPaths();
            var exportPaths = new List<string>();

            foreach (var assetPath in assetsPaths)
            {
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                var labels = AssetDatabase.GetLabels(asset);
                var add = true;

                if (assetPath.Contains("ProjectSettings"))
                    continue;
                if (assetPath.Contains("Library"))
                    continue;
                if (assetPath.Contains("UnityExtensions"))
                    continue;

                foreach (var label in ExcludeList)
                {
                    if (labels.Contains(label))
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                {
                    if (!labels.Contains(Application.productName))
                    {
                        var llist = labels.ToList();
                        llist.Add(Application.productName);
                        AssetDatabase.SetLabels(asset, llist.ToArray());
                    }
                        

                    exportPaths.Add(assetPath);
                }
            }

            AssetDatabase.ExportPackage(exportPaths.ToArray(), "Release/" + Application.productName + ".unitypackage");
            Debug.Log("Package exported.");
        }
    }
}