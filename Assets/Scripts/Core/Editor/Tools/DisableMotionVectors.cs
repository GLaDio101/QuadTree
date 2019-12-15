using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Core.Editor.Tools
{
  public class DisableMotionVectors : UnityEditor.Editor
  {
    [MenuItem("Tools/Extra/Disable Motion Vectors in Scene")]
    public static void DisableMotionVectorsScene()
    {
      MeshRenderer[] componentsInChildren = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();

      foreach (MeshRenderer component in componentsInChildren)
      {
        if (component.motionVectorGenerationMode != MotionVectorGenerationMode.ForceNoMotion)
          component.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
      }

      EditorSceneManager.MarkAllScenesDirty();
    }

    [MenuItem("Tools/Extra/Disable Motion Vectors in Project")]
    public static void DisableMotionVectorsProject()
    {
      var assetsPaths = AssetDatabase.GetAllAssetPaths();
      var prefabsPaths = new List<string>();
      foreach (var assetPath in assetsPaths)
      {
        if (assetPath.Contains(".prefab"))
        {
          prefabsPaths.Add(assetPath);
        }
      }

      foreach (var prefabsPath in prefabsPaths)
      {
        var prefab = AssetDatabase.LoadAssetAtPath(prefabsPath, typeof(GameObject));

        var newObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        var update = false;
        if (newObject != null)
        {
          MeshRenderer[] components = newObject.GetComponentsInChildren<MeshRenderer>();

          foreach (MeshRenderer component in components)
          {
            if (component.motionVectorGenerationMode != MotionVectorGenerationMode.ForceNoMotion)
            {
              component.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
              Debug.Log(component.gameObject.name);
              update = true;
            }
          }
        }

        if (update)
        {
          Debug.Log(prefabsPath + " updated.");
        }

        DestroyImmediate(newObject);
      }
    }
  }
}