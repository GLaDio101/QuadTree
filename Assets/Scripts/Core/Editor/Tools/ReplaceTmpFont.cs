using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class ReplaceTmpFont : ScriptableWizard
    {
       public TMP_FontAsset From;

        public TMP_FontAsset To;

        public int SizePadding = 0;

        [MenuItem("Tools/Extra/Replace Tmp Font")]
        [UsedImplicitly]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Replace Tmp Font", typeof(ReplaceTmpFont), "Replace");
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
                    TextMeshProUGUI[] components = newObject.GetComponentsInChildren<TextMeshProUGUI>();

                    foreach (TextMeshProUGUI component in components)
                    {
                        if (component.font == From)
                        {
                            update = true;
                            component.font = To;
                            if (component.enableAutoSizing)
                            {
                                component.fontSizeMax += SizePadding;
                                component.fontSizeMin += SizePadding;
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