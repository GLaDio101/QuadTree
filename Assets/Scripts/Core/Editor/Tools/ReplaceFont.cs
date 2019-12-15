using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Editor.Tools
{
    public class ReplaceFont : ScriptableWizard
    {
        public Font From;

        public Font To;

        public int SizePadding = 0;

        [MenuItem("Tools/Extra/Replace Font")]
        [UsedImplicitly]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Replace Font", typeof(ReplaceFont), "Replace");
        }

        [UsedImplicitly]
        private void OnWizardCreate()
        {
            var assetsPaths = AssetDatabase.GetAllAssetPaths();
            var prefabsPaths = new List<string>();
            foreach (var assetPath in assetsPaths)
            {
                if (assetPath.Contains(".prefab"))
                {
                    prefabsPaths.Add(assetPath);
                }
            }

            foreach (var prefabsPath in prefabsPaths)
            {
                var prefab = AssetDatabase.LoadAssetAtPath(prefabsPath, typeof(GameObject));

                var newObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                var update = false;
                if (newObject != null)
                {
                    Text[] components = newObject.GetComponentsInChildren<Text>();

                    foreach (Text component in components)
                    {
                        if (component.font == From)
                        {
                            update = true;
                            component.font = To;
                            if (component.resizeTextForBestFit)
                            {
                                component.resizeTextMaxSize += SizePadding;
                                component.resizeTextMinSize += SizePadding;
                            }

                            component.fontSize += SizePadding;
                        }
                    }
                }

                if (update)
                {
                    Debug.Log(prefabsPath + " updated.");
                }
                    
                DestroyImmediate(newObject);
            }
        }
    }
}