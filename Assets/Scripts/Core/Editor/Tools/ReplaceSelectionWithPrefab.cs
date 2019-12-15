using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tools
{
    public class ReplaceSelectionWithPrefab : ScriptableWizard
    {
        public GameObject Prefab;

        [MenuItem("Tools/Game Object/Replace Selection with Prefab #&r")]
        [UsedImplicitly]
        public static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Replace Selection with Prefab", typeof(ReplaceSelectionWithPrefab), "Replace");
        }

        [UsedImplicitly]
        private void OnWizardCreate()
        {
            var replaces = Selection.objects;

            foreach (var t in replaces)
            {
                var newObject = PrefabUtility.InstantiatePrefab(Prefab) as GameObject;
                if (newObject != null)
                {
                    newObject.transform.SetParent(((GameObject)t).transform.parent);
                    newObject.transform.position = ((GameObject)t).transform.position;
                    newObject.transform.rotation = ((GameObject)t).transform.rotation;
                    newObject.transform.localScale = ((GameObject)t).transform.localScale;
                }

                Undo.RegisterCreatedObjectUndo(newObject, "Replacement Object");

                Undo.DestroyObjectImmediate(t);
            }
        }
    }
}