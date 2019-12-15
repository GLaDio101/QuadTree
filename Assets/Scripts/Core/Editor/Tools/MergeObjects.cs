using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class MergeObjects : UnityEditor.Editor
    {
        [MenuItem("Tools/Optimization/Merge Objects")]// % – CTRL | # – Shift | & – Alt
        [UsedImplicitly]
        public static void MergeObjectsOperation()
        {
            var meshParent = Selection.activeGameObject;

            // check if there is children
            if (meshParent.transform.childCount < 2)
            {
                Debug.LogWarning("No child objects merge.");
                return;
            }

            // chek if those children has mesh filters
            MeshFilter[] meshFilters = meshParent.GetComponentsInChildren<MeshFilter>();
            if (meshFilters.Length < 2)
            {
                Debug.LogWarning("No child meshes merge.");
                return;
            }

            // get material
            var meshObject = new GameObject("Mesh");
            meshObject.transform.SetParent(meshParent.transform);
            var material = meshParent.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial;

            Undo.RegisterCreatedObjectUndo(meshObject, "Merge Objects");

            // loop and merge meshes
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                Undo.DestroyObjectImmediate(meshFilters[i].gameObject.GetComponent<MeshRenderer>());
                Undo.DestroyObjectImmediate(meshFilters[i]);
                i++;
            }

            // create components and set merged mesh and material
            var meshFilter = meshObject.AddComponent<MeshFilter>();
            var meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = material;
            meshFilter.mesh = new Mesh { name = meshParent.name };
            meshFilter.sharedMesh.CombineMeshes(combine, true, true);

            AutoWeld(meshFilter.sharedMesh, 0.3f);

            EditorSceneManager.MarkAllScenesDirty();
        }

        private static void UpdatePivot(MeshFilter meshFilter)
        {
            Mesh mesh = meshFilter.sharedMesh;

            Bounds b = mesh.bounds;
            Vector3 offset = -1 * b.center;
            Vector3 lastP = new Vector3(offset.x / b.extents.x, offset.y / b.extents.y, offset.z / b.extents.z);

            Vector3 diff = Vector3.Scale(mesh.bounds.extents, lastP - new Vector3(1, 1, -1));

            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i] += diff;
            }
            mesh.vertices = verts;
            mesh.RecalculateBounds();
        }

        private static void AutoWeld(Mesh mesh, float threshold)
        {
            Vector3[] verts = mesh.vertices;
            int[] tris = mesh.triangles;
            Vector2[] uvs = mesh.uv;
            Vector3[] normals = mesh.normals;
            Vector3[] newVerts = verts.Clone() as Vector3[];

            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 curVert = verts[i];
                for (int j = i + 1; j < verts.Length; j++)
                {
                    if (Vector3.Distance(curVert, newVerts[j]) < threshold)
                    {
                        newVerts[j] = curVert;
                    }
                }
            }

            mesh.Clear();
            mesh.vertices = newVerts;
            mesh.triangles = tris;
            mesh.uv = uvs;
            mesh.normals = normals;
            mesh.RecalculateBounds();
        }
    }
}