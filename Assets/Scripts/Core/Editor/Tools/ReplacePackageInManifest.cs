using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class ReplacePackageInManifest : UnityEditor.Editor
    {
        [MenuItem("Tools/Extra/Replace Package in Manifests")]
        public static void Execute()
        {
            string[] allfiles = System.IO.Directory.GetFiles(Application.dataPath, "*.xml", System.IO.SearchOption.AllDirectories);

            foreach (string allfile in allfiles)
            {
                if (allfile.Contains("Plugins") && allfile.Contains("Android") && allfile.Contains("Manifest"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(allfile);
                    string packageName = doc.DocumentElement.GetAttribute("package");
                    Debug.Log(packageName);
                    if (packageName != "com.unity3d.player")
                    {
                        doc.DocumentElement.SetAttribute("package", Application.identifier);
                        Debug.Log(doc.DocumentElement.GetAttribute("package"));
                        doc.Save(allfile);
                    }
                }
            }
        }
    }
}