using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class FixMeshObjectFromName : UnityEditor.Editor
    {
        [MenuItem("Tools/Extra/Fix Mesh Object From NameLabel")] // % – CTRL | # – Shift | & – Alt | _ - for single
        [UsedImplicitly]
        private static void ResetMeshOperation()
        {
            var replaces = Selection.objects;

            foreach (GameObject t in replaces)
            {
                Debug.Log(t.name);
                MeshFilter renderer = t.GetComponent<MeshFilter>();
                MeshCollider collider = t.GetComponent<MeshCollider>();

                string[] results = AssetDatabase.FindAssets(t.name + " t:Mesh");
                Object[] meshes = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(results[0]));
                foreach (var mesh in meshes)
                {
                    if (mesh.name == t.name && mesh is Mesh)
                    {
                        renderer.mesh = mesh as Mesh;
                        collider.sharedMesh = mesh as Mesh;
                        break;
                    }
                }
            }
        }
    }
}