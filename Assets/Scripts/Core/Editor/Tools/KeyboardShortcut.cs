using JetBrains.Annotations;
using UnityEditor;

namespace Core.Editor.Tools
{
    public class KeyboardShortcut : UnityEditor.Editor
    {
        [MenuItem("Settings/Player Settings _F9")] // % – CTRL | # – Shift | & – Alt | _ - for single
        [UsedImplicitly]
        private static void OpenPlayerSettings()
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
        }

        [MenuItem("Settings/Quality Settings _F10")]
        [UsedImplicitly]
        private static void OpenQualitySettings()
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Quality");
        }

        [MenuItem("Settings/Graphics Settings _F11")]
        [UsedImplicitly]
        private static void OpenGraphicsSettings()
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Graphics");
        }

        [MenuItem("Settings/Editor Settings")]
        [UsedImplicitly]
        private static void OpenEditorSettings()
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Editor");

        }

        [MenuItem("Settings/Physics Settings")]
        [UsedImplicitly]
        private static void OpenPhysicsSettings()
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Physics");
        }

        [MenuItem("Settings/Time Settings")]
        [UsedImplicitly]
        private static void OpenTimeSettings()
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Time");
        }
    }
}