using System;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Extensions
{
    public class AudioAutoplay : EditorWindow
    {

        [MenuItem("Tools/Audio Autoplay")]
        [UsedImplicitly]
        private static void Init()
        {
            var window = EditorWindow.GetWindow(typeof(AudioAutoplay));
            window.Show();
        }

        [UsedImplicitly]
        private void OnGUI()
        {
            GUILayout.Label("Audio files will now play on selection change.");
        }

        [UsedImplicitly]
        private void OnSelectionChange()
        {
            UnityEngine.Object[] clips = Selection.GetFiltered(typeof(AudioClip), SelectionMode.Unfiltered);

            if (clips != null && clips.Length == 1)
            {
                AudioClip clip = (AudioClip)clips[0];
                PlayClip(clip);
            }
        }

        public static void PlayClip(AudioClip clip)
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] {
             typeof(AudioClip)
            },
            null
            );
            method.Invoke(
                null,
                new object[] {
             clip
            }
            );
        }
    }
}