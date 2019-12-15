using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Standard_Assets.Core.FindMissingComponents.Editor
{
    public class FindMissingComponents
    {
        public static RectTransform[] _transforms;

        static List<Object> offenders = new List<Object>();

        [MenuItem("Tools/Find Missing Components")]
        private static void FindMissingComponentsFunction()
        {
            _transforms = Object.FindObjectsOfType<RectTransform>();
            offenders.Clear();
            foreach (var item in _transforms)
            {
                //Debug.Log("checking: " + item.name);
                CheckObject(item.gameObject);
            }

            Selection.objects = offenders.ToArray();

            Debug.Log("Found " + offenders.Count.ToString() + " objects with missing components");
        }

        private static void CheckObject(GameObject go)
        {
            Component[] comps = go.GetComponents<Component>();
            foreach (var item in comps)
            {
                if (item == null)
                {
                    offenders.Add(go as global::UnityEngine.Object);
                    break;
                }
            }
        }

        [MenuItem("Tools/Select Missing Components", true)]
        [UsedImplicitly]
        static bool ValidateFindMissingComponents()
        {
            return true;
        }

    }
}