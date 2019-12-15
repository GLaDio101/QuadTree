using System.IO;
using System.Text.RegularExpressions;
using Core.Editor.Code;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class CheckListenerOnMediators : UnityEditor.Editor
    {
        [MenuItem("Tools/Check Listeners In Mediators")] // % – CTRL | # – Shift | & – Alt | _ - for single
        [UsedImplicitly]
        private static void ResetMeshOperation()
        {
            string[] allfiles = Directory.GetFiles(Application.dataPath + "/Scripts", "*.cs", SearchOption.AllDirectories);

            Debug.Log(allfiles.Length);
            foreach (string path in allfiles)
            {
                var text = CodeUtilities.LoadScript(path);
                int addCount = new Regex(Regex.Escape(".AddListener(")).Matches(text).Count;
                int removeCount = new Regex(Regex.Escape(".RemoveListener(")).Matches(text).Count;
                if(!path.Contains("Mediator.cs"))
                    continue;
                if (addCount > 0)
                {
                    Debug.Log(path);
                    Debug.Log(addCount +" - " + removeCount);
                    if(addCount > removeCount)
                        Debug.LogError(path);
                }
            }
        }
    }
}