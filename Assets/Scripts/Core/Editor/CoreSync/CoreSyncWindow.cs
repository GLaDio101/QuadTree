using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Core.Editor.CoreSync
{
  public class CoreSyncWindow : EditorWindow
  {
    private CoreSyncSettings _data;

    private GUIStyle _titleStyle;

    public IDictionary<string, LinkedProject> Map;

    private int _selectedIndexInit;

    private int _selectedIndexUpdate;

    [MenuItem("BrosWindow/Core Sync")]
    [UsedImplicitly]
    private static void Init()
    {
      var window = EditorWindow.GetWindow(typeof(CoreSyncWindow));
      window.Show();
    }

    [UsedImplicitly]
    private void OnEnable()
    {
      titleContent.text = "Core Sync";
      titleContent.tooltip = "Tools for syncronizing core libraries between projects.";

      minSize = new Vector2(285f, 285f);

      _data = AssetDatabase.LoadAssetAtPath<CoreSyncSettings>(CoreSyncSettings.DataPath);

      if (_data == null)
        return;

      // map values
      Map = new Dictionary<string, LinkedProject>();
      foreach (var linkedProject in _data.List)
      {
        Map.Add(linkedProject.Name, linkedProject);
      }
    }

    [UsedImplicitly]
    private void OnGUI()
    {
      if (_data == null)
        return;

      _titleStyle = EditorStyles.helpBox;
      _titleStyle.alignment = TextAnchor.UpperCenter;
      _titleStyle.fontSize = 12;
      _titleStyle.fontStyle = FontStyle.Bold;

      if (Application.identifier == "com.typhoon.gameservices")
      {
        Core();
      }
      else
      {
        Linked();
      }
    }

    private void Linked()
    {
      GUILayout.BeginVertical(EditorStyles.helpBox);
      GUILayout.Label("Synced Folders", _titleStyle);
      foreach (string folder in _data.SyncFolders)
      {
        GUILayout.Label(folder);
      }

      if (GUILayout.Button("Remove All"))
      {
        DeleteSyncFolders();
      }

      GUILayout.EndVertical();
    }

    private void Core()
    {
      GUILayout.BeginVertical(EditorStyles.helpBox);
      GUILayout.Label("Project", _titleStyle);
      if (GUILayout.Button("Link"))
      {
        AddNewProject();
      }

      string[] projectNames = _data.List.Where(project => !project.IsInited).Select(project => project.Name).ToArray();
      if (projectNames.Length > 0)
      {
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        _selectedIndexInit = EditorGUILayout.Popup(_selectedIndexInit, projectNames);
        GUI.enabled = Map.ContainsKey(projectNames[_selectedIndexInit]);
        if (GUILayout.Button("Initialize"))
        {
          InitProject(Map[projectNames[_selectedIndexInit]]);
        }

        GUI.enabled = true;
        GUILayout.EndHorizontal();
      }

      projectNames = _data.List.Where(project => project.IsInited).Select(project => project.Name).ToArray();
      if (projectNames.Length > 0)
      {
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        _selectedIndexUpdate = EditorGUILayout.Popup(_selectedIndexUpdate, projectNames);
        if (GUILayout.Button("Update"))
        {
          UpdateProject(Map[projectNames[_selectedIndexUpdate]]);
        }

        GUILayout.EndHorizontal();
      }

      GUILayout.EndVertical();
    }

    private void AddNewProject()
    {
      string path = EditorUtility.OpenFolderPanel("Project Folder", "", "projectName");

      // parse project info
      var arr = path.Split('/');
      var projectName = arr[arr.Length - 1];

      // check if project is already linked
      if (Map.ContainsKey(projectName))
      {
        Debug.LogWarning("Already linked.");
        return;
      }

      // check if project is already inited
      string[] files = Directory.GetFiles(path);
      bool alreadyInitied = true;
      foreach (string file in files)
      {
        if (file.Contains(".gitignore"))
        {
          alreadyInitied = false;
          break;
        }
      }

      // create link
      var linkedProject = new LinkedProject
      {
        Name = projectName,
        Path = path,
        IsInited = !alreadyInitied
      };
      _data.List.Add(linkedProject);
      Map.Add(linkedProject.Name, linkedProject);

      EditorUtility.SetDirty(_data);
      AssetDatabase.SaveAssets();
    }

    private void InitProject(LinkedProject linkedProject)
    {
      var ignorePath = Application.dataPath + "/../.gitignore";
      FileUtil.CopyFileOrDirectory(ignorePath, linkedProject.Path + "/.gitignore");

      foreach (string folder in _data.InitialFolders)
      {
        Directory.CreateDirectory(linkedProject.Path + "/" + folder);
      }

      linkedProject.IsInited = true;

      EditorUtility.SetDirty(_data);
      AssetDatabase.SaveAssets();

      CopyPlugins(linkedProject);

      UpdateProject(linkedProject);
    }

    private void DeleteSyncFolders()
    {
      foreach (string folder in _data.SyncFolders)
      {
        var target = Application.dataPath + "/../Assets/" + folder;
        if (Directory.Exists(target))
        {
          FileUtil.DeleteFileOrDirectory(target);
        }
      }
    }

    private void UpdateProject(LinkedProject linkedProject)
    {
      try
      {
        foreach (string folder in _data.SyncFolders)
        {
          var target = Application.dataPath + "/../Assets/" + folder;
          var workingDir = linkedProject.Path + "/Assets";
          var path = Application.dataPath + "/Scripts/Core/Editor/CoreSync/createJunction.bat";
          var args = "\"" + workingDir + "\" \"" + folder + "\" \"" + target + "\"";

          var foo = new Process
          {
            StartInfo =
            {
              FileName = path,
              Arguments = args,
              WindowStyle = ProcessWindowStyle.Hidden
            }
          };
          foo.Start();
        }
      }
      catch (Exception e)
      {
        Debug.LogWarning("You have to open editor in administrator mode to do this.");
        Debug.LogWarning(e.Message);
      }
    }

    private void CopyPlugins(LinkedProject linkedProject)
    {
      var from = Application.dataPath + "/../Assets/Plugins";
      var to = linkedProject.Path + "/Assets/Plugins";
      FileUtil.CopyFileOrDirectory(from, to);
    }
  }
}