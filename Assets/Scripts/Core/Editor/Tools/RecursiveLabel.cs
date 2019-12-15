using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Editor.Tools
{
    internal class RecursiveLabel : EditorWindow
    {
        internal enum LabelAction
        {
            Append,
            Remove,
            Overwrite,
            Clear
        }

        private string _label = "";
        public LabelAction Action = LabelAction.Append;
        public string[] Paths = new string[0];

        private static string[] GetSelectedPaths()
        {
            var paths = new List<string>();

            foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path)) continue; ;

                var fullPath = Path.GetFullPath(path);
                if (!File.Exists(fullPath) && !Directory.Exists(fullPath)) continue;

                paths.Add(path);
            }

            return paths.ToArray();
        }

        private static void ShowWindow(LabelAction action, string[] paths)
        {
            var window = GetWindow<RecursiveLabel>();
            window.Action = action;
            window.Paths = paths;
            window.maxSize = window.minSize = new Vector2(300, 50);
            window.Show();
        }

        [MenuItem("Tools/Recursive Label/Append Labels")]
        [UsedImplicitly]
        private static void AppendLabelsMenu()
        {
            ShowWindow(LabelAction.Append, GetSelectedPaths());
        }

        [MenuItem("Tools/Exclude From Package")]
        [UsedImplicitly]
        private static void ExcludeFromPackageOperation()
        {
            SetLabels("Exclude", GetSelectedPaths());
        }

        [MenuItem("Tools/Recursive Label/Remove Labels")]
        [UsedImplicitly]
        private static void RemoveLabelsMenu()
        {
            ShowWindow(LabelAction.Remove, GetSelectedPaths());
        }

        [MenuItem("Tools/Recursive Label/Overwrite Labels")]
        [UsedImplicitly]
        private static void OverwriteLabelsMenu()
        {
            ShowWindow(LabelAction.Overwrite, GetSelectedPaths());
        }

        [MenuItem("Tools/Recursive Label/Clear Labels")]
        [UsedImplicitly]
        private static void ClearLabelsMenu()
        {
            SetLabels(null, GetSelectedPaths(), LabelAction.Clear);
        }

        [UsedImplicitly]
        private void OnEnable()
        {
            titleContent.text = "Recursive Label: "+ Action;
            titleContent.tooltip = "Set label to files and folders recursively";
        }

        [UsedImplicitly]
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Labels (comma-separated): ");
            _label = GUILayout.TextField(_label);
            GUILayout.EndHorizontal();

            if (GUILayout.Button(Action +"!"))
            {
                Close();
                SetLabels(_label, Paths, Action);
            }
        }

        private static void AppendLabels(Object asset, string[] labels)
        {
            var currentLabels = AssetDatabase.GetLabels(asset);
            var newLabels = new List<string>(currentLabels);
            newLabels.AddRange(labels);

            AssetDatabase.SetLabels(asset, newLabels.ToArray());
        }

        private static void RemoveLabels(Object asset, string[] labels)
        {
            var currentLabels = AssetDatabase.GetLabels(asset);
            var removeLabels = new List<string>(labels);

            var newLabels = new List<string>();
            for (int i = 0; i < currentLabels.Length; i++)
            {
                if(removeLabels.IndexOf(currentLabels[i]) == -1) newLabels.Add(currentLabels[i]);
            }
            
            AssetDatabase.SetLabels(asset, newLabels.ToArray());
        }

        private static void SetLabels(string labels, string[] paths, LabelAction action = LabelAction.Append)
        {
            var labelArray = labels == null ? new string[0] : labels.Split(',');
            if (action != LabelAction.Clear && labelArray.Length == 0) return;

            var assets = AssetDatabase.FindAssets("", paths);
            foreach (var asset in assets)
            {
                var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(asset), typeof(Object));
                if (obj == null) continue;

                switch (action)
                {
                    case LabelAction.Overwrite:
                        AssetDatabase.SetLabels(obj, labelArray);
                        break;

                    case LabelAction.Clear:
                        AssetDatabase.ClearLabels(obj);
                        break;

                    case LabelAction.Remove:
                        RemoveLabels(obj, labelArray);
                        break;

                    default:
                        AppendLabels(obj, labelArray);
                        break;
                }
            }

            foreach (var path in paths)
            {
                var obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                if (obj == null) continue;

                switch (action)
                {
                    case LabelAction.Overwrite:
                        AssetDatabase.SetLabels(obj, labelArray);
                        break;

                    case LabelAction.Clear:
                        AssetDatabase.ClearLabels(obj);
                        break;

                    case LabelAction.Remove:
                        RemoveLabels(obj, labelArray);
                        break;

                    default:
                        AppendLabels(obj, labelArray);
                        break;
                }
            }

            AssetDatabase.SaveAssets();
            Resources.UnloadUnusedAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            Debug.Log(action +" Labels: Done!");
        }
    }
}