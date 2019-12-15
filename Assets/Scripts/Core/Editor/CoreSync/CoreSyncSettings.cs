using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.CoreSync
{
  [Serializable]
  public class CoreSyncSettings : ScriptableObject
  {
    [NonSerialized] public const string DataPath = "Assets/Scripts/Core/Editor/CoreSync/CoreSyncSettings.asset";

    public List<LinkedProject> List;

    public List<string> InitialFolders;

    public List<string> SyncFolders;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/BrosData/CoreSyncSettings")]
    public static void CreateMyAsset()
    {
      CoreSyncSettings asset = ScriptableObject.CreateInstance<CoreSyncSettings>();

      AssetDatabase.CreateAsset(asset, DataPath);
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }
#endif
  }
}