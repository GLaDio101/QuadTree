#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.IO;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace Core.Manager.Audio
{
    public class AudioManager : EventView
    {
        public string RootPath = "Assets/Audio";

        public AudioSource[] sourceList;

        public AudioSource ThemeSong;

        public AudioMixer mixer;

#if UNITY_EDITOR
        [ContextMenu("Reload Sounds")]
        void ReloadSounds()
        {
            AudioClip[] audioClipList = GetAtPath<AudioClip>(RootPath);

            // clear olds
            var count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            //// create new ones
            for (int i = 0; i < sourceList.Length; i++)
            {
                var clip = audioClipList[i];
                GameObject obj = new GameObject(clip.name);
                obj.transform.SetParent(transform);
                AudioSource comp = obj.AddComponent<AudioSource>();
                comp.clip = clip;
                comp.playOnAwake = false;
                var name = clip.name.Split('_')[0];
                sourceList[int.Parse(name) - 1] = comp;
            }

            // sort
            foreach (Transform child in transform)
            {
                var name = child.gameObject.name.Split('_')[0];
                child.SetSiblingIndex(int.Parse(name) - 1);
            }
        }

        public static T[] GetAtPath<T>(string path)
        {
            ArrayList al = new ArrayList();
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
            foreach (string fileName in fileEntries)
            {
                int index = fileName.Replace("\\", "/").LastIndexOf("/", StringComparison.Ordinal);
                string localPath = "Assets/" + path;

                if (index > 0)
                    localPath += fileName.Substring(index);

                Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

                if (t != null)
                    al.Add(t);
            }
            T[] result = new T[al.Count];
            for (int i = 0; i < al.Count; i++)
                result[i] = (T)al[i];

            return result;
        }
#endif
    }
}
