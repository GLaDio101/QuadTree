using System.Text.RegularExpressions;
using Core.Editor.Code;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Test
{
    public class ExcludeScriptFromProject : UnityEditor.Editor
    {
        [MenuItem("Tools/Exclude Script From Project")] // % – CTRL | # – Shift | & – Alt | _ - for single
        [UsedImplicitly]
        public static void ExcludeScriptFromProjectOperation()
        {
            foreach (var item in Selection.objects)
            {
                if (!(item is TextAsset)) continue;

                var script = item as TextAsset;
                var path = AssetDatabase.GetAssetPath(script);
                var text = CodeUtilities.LoadScript(path);
                if (!text.StartsWith("#if UNITY_EDITOR"))
                {
                    text = "#if UNITY_EDITOR \r\n" + text + "\r\n#endif";
                    text = Regex.Replace(text, @"\r\n|\n\r|\n|\r", "\r\n");
                    CodeUtilities.SaveFile(text, path);
                }
            }
        }
    }
}