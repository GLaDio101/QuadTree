using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class PanelLayoutEditor : UnityEditor.Editor
    {
        [MenuItem("Tools/Rect/Div Hor %#&1")] // % – CTRL | # – Shift | & – Alt | _ - for single
        [UsedImplicitly]
        private static void DivHor()
        {
            var gameObjects = Selection.gameObjects;

            if (gameObjects.Length == 0)
            {
                Debug.LogWarning("Select objects to div.");
                return;
            }


            foreach (GameObject go in gameObjects)
            {
                RectTransform goTrans = go.transform as RectTransform;

                if (goTrans == null)
                    continue;
                if (goTrans.parent == null)
                    continue;

                GameObject div = new GameObject(go.name);
                RectTransform divTrans = div.AddComponent<RectTransform>();

                divTrans.SetParent(goTrans.parent);
                divTrans.SetSiblingIndex(goTrans.GetSiblingIndex() + 1);
                divTrans.localScale = Vector3.one;

                float top = goTrans.anchorMin.y;
                float bottom = goTrans.anchorMax.y;
                float diff = (bottom - top) / 2;

                divTrans.anchorMin = new Vector2(goTrans.anchorMin.x, bottom - diff);
                divTrans.anchorMax = new Vector2(goTrans.anchorMax.x, bottom);

                divTrans.offsetMin = new Vector2(0, 0);
                divTrans.offsetMax = new Vector2(0, 0);

                goTrans.anchorMin = new Vector2(goTrans.anchorMin.x, top);
                goTrans.anchorMax = new Vector2(goTrans.anchorMax.x, top + diff);

                goTrans.offsetMin = new Vector2(0, 0);
                goTrans.offsetMax = new Vector2(0, 0);
            }

            EditorSceneManager.MarkAllScenesDirty();
        }

        [MenuItem("Tools/Rect/Div Ver %#&2")] // % – CTRL | # – Shift | & – Alt | _ - for single
        [UsedImplicitly]
        private static void DivVer()
        {
            var gameObjects = Selection.gameObjects;

            if (gameObjects.Length == 0)
            {
                Debug.LogWarning("Select objects to div.");
                return;
            }


            foreach (GameObject go in gameObjects)
            {
                RectTransform goTrans = go.transform as RectTransform;

                if (goTrans == null)
                    continue;
                if (goTrans.parent == null)
                    continue;

                GameObject div = new GameObject(go.name);
                RectTransform divTrans = div.AddComponent<RectTransform>();

                divTrans.SetParent(goTrans.parent);
                divTrans.SetSiblingIndex(goTrans.GetSiblingIndex() + 1);
                divTrans.localScale = Vector3.one;

                float left = goTrans.anchorMin.x;
                float right = goTrans.anchorMax.x;
                float diff = (right - left) / 2;

                divTrans.anchorMin = new Vector2(left + diff, goTrans.anchorMin.y);
                divTrans.anchorMax = new Vector2(right, goTrans.anchorMax.y);

                divTrans.offsetMin = new Vector2(0, 0);
                divTrans.offsetMax = new Vector2(0, 0);

                goTrans.anchorMin = new Vector2(left, goTrans.anchorMin.y);
                goTrans.anchorMax = new Vector2(left + diff, goTrans.anchorMax.y);

                goTrans.offsetMin = new Vector2(0, 0);
                goTrans.offsetMax = new Vector2(0, 0);
            }

            EditorSceneManager.MarkAllScenesDirty();
        }
    }
}