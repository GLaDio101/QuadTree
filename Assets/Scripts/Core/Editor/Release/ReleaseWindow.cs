using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Release
{
  public class ReleaseWindow : EditorWindow
  {
    private static string _password = "SF_2015";

    private string _nextVersion;

    private int _nextVersionCode;

    private GUIStyle _titleStyle;

    [MenuItem("BrosWindow/Release")]
    [UsedImplicitly]
    private static void Init()
    {
      ReleaseWindow window = (ReleaseWindow) GetWindow(typeof(ReleaseWindow));
      PlayerSettings.keyaliasPass = PlayerSettings.keystorePass = _password;
      window.Show();
    }

    [UsedImplicitly]
    private void OnEnable()
    {
      titleContent.text = "Release";
      titleContent.tooltip = "For production builds.";
      var version = PlayerSettings.Android.bundleVersionCode;
      _nextVersionCode = version + 1;
      version++;
      _nextVersion = Mathf.FloorToInt(version / 100f) + "." + version % 100;
      PlayerSettings.keyaliasPass = PlayerSettings.keystorePass = _password;
    }

    [UsedImplicitly]
    private void OnGUI()
    {
      _titleStyle = EditorStyles.helpBox;
      _titleStyle.alignment = TextAnchor.UpperCenter;
      _titleStyle.fontSize = 12;
      _titleStyle.fontStyle = FontStyle.Bold;

      GUILayout.Label("Release and Run", _titleStyle);
      EditorGUILayout.BeginVertical(EditorStyles.helpBox);
      PublishSettings.BundlePath = EditorGUILayout.TextField("Bundle Path : ", PublishSettings.BundlePath);
      PublishSettings.BundleRootPath = EditorGUILayout.TextField("Bundle Root Path : ", PublishSettings.BundleRootPath);
      _password = EditorGUILayout.TextField("Password : ", _password);
      PublishSettings.SceneName = EditorGUILayout.TextField("Scene Name : ", PublishSettings.SceneName);
      PublishSettings.HostSceneName = EditorGUILayout.TextField("Host Scene Name : ", PublishSettings.HostSceneName);
      PublishSettings.BuildPath = EditorGUILayout.TextField("Release Path : ", PublishSettings.BuildPath);
      PublishSettings.BuildBundles = EditorGUILayout.Toggle("Build Bundles : ", PublishSettings.BuildBundles);

      var buttonLabel = "Next Release v" + _nextVersion + " - " + _nextVersionCode;
      if (GUILayout.Button(buttonLabel))
      {
        PlayerSettings.SplashScreen.showUnityLogo = false;
        PlayerSettings.Android.bundleVersionCode = _nextVersionCode;
        PlayerSettings.bundleVersion = _nextVersion;
        ReleaseAndroid();
      }

      buttonLabel = "Release v" + PlayerSettings.bundleVersion + " - " + PlayerSettings.Android.bundleVersionCode;
      if (GUILayout.Button(buttonLabel))
      {
        PlayerSettings.SplashScreen.showUnityLogo = false;
        ReleaseAndroid();
      }

      EditorGUILayout.Space();
      buttonLabel = "Development Run v" + PlayerSettings.bundleVersion + " - " +
                    PlayerSettings.Android.bundleVersionCode;
      if (GUILayout.Button(buttonLabel))
      {
        PlayerSettings.SplashScreen.showUnityLogo = false;
        PlayerSettings.keyaliasPass = PlayerSettings.keystorePass = _password;
        PublishSettings.BuildOption =
          BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.AutoRunPlayer;
        Publish.Release();
      }

      buttonLabel = "Run v" + PlayerSettings.bundleVersion + " - " +
                    PlayerSettings.Android.bundleVersionCode;
      if (GUILayout.Button(buttonLabel))
      {
        PlayerSettings.SplashScreen.showUnityLogo = false;
        PlayerSettings.keyaliasPass = PlayerSettings.keystorePass = _password;
        PublishSettings.BuildOption = BuildOptions.AutoRunPlayer;
        Publish.Release();
      }

      EditorGUILayout.Space();
      if (GUILayout.Button("Build Bundles"))
      {
        Publish.BuildBundles();
      }

      EditorGUILayout.EndVertical();

      EditorGUILayout.Space();
      EditorGUILayout.BeginVertical(EditorStyles.helpBox);
      GUILayout.Label("Multiplayer Test", _titleStyle);
      EditorGUILayout.BeginHorizontal();
      if (GUILayout.Button("Build Host"))
      {
        PublishSettings.BuildOption =
          BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.AutoRunPlayer;
#if !UNITY_EDITOR_OSX
        Publish.Build(BuildTarget.StandaloneWindows64, PublishSettings.HostSceneName, "-host");
#else
        Publish.Build(BuildTarget.StandaloneOSX, PublishSettings.HostSceneName, "-host");
        #endif
      }

      if (GUILayout.Button("Run Host >"))
      {
#if !UNITY_EDITOR_OSX
        Publish.Run(BuildTarget.StandaloneWindows64, "-host");
#else
        Publish.Run(BuildTarget.StandaloneOSX,"-host");
        #endif
      }

      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
      if (GUILayout.Button("Release Host (Linux)"))
      {
        PublishSettings.BuildOption = BuildOptions.EnableHeadlessMode;
        Publish.Build(BuildTarget.StandaloneLinux64, PublishSettings.HostSceneName, "-host");
      }

      EditorGUILayout.Space();
      EditorGUILayout.BeginHorizontal();
      if (GUILayout.Button("Build Client"))
      {
        PublishSettings.BuildOption =
          BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.AutoRunPlayer;
#if !UNITY_EDITOR_OSX
        Publish.Build(BuildTarget.StandaloneWindows64, PublishSettings.SceneName);
#else
        Publish.Build(BuildTarget.StandaloneOSX, PublishSettings.SceneName);
#endif
      }

      if (GUILayout.Button("Run Client >"))
      {
#if !UNITY_EDITOR_OSX
        Publish.Run(BuildTarget.StandaloneWindows64);
#else
        Publish.Run(BuildTarget.StandaloneOSX);
        #endif
      }

      EditorGUILayout.EndHorizontal();
      EditorGUILayout.EndVertical();
    }

    private void ReleaseAndroid()
    {
      PlayerSettings.keyaliasPass = PlayerSettings.keystorePass = _password;
      PublishSettings.BuildOption = BuildOptions.None;
      Publish.Release();
    }
  }
}