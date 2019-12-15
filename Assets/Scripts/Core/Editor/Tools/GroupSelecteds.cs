using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class GroupSelecteds : UnityEditor.Editor
    {
        [MenuItem("Tools/Game Object/Group Selecteds %#g")] // % – CTRL | # – Shift | & – Alt | _ - for single
        [UsedImplicitly]
        private static void GroupSelectedsOperation()
        {
            var gameObjects = Selection.gameObjects;

            if (gameObjects.Length == 0)
            {
                Debug.LogWarning("Select objects to group.");
                return;
            }

            GameObject parent = new GameObject("Group");

            Undo.RegisterCreatedObjectUndo(parent, "Group SelectedObjects");

            if (gameObjects[0].transform.parent != null)
                parent.transform.SetParent(gameObjects[0].transform.parent);

            foreach (GameObject go in gameObjects)
            {
                //go.transform.SetParent(parent.transform);
                Undo.SetTransformParent(go.transform, parent.transform, "Group SelectedObjects");
            }

            EditorSceneManager.MarkAllScenesDirty();
        }
    }
}