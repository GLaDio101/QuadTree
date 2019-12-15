using System;
using System.Collections;
using System.Collections.Generic;
using Core.Manager.Bundle;
using Core.Promise;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Utility = AssetBundles.Utility;

namespace Core.Manager.Scene
{
    public class SceneModel : ISceneModel
    {
        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject contextView { get; set; }

        private Dictionary<SceneLayer, LoadData> _layerDataMap;

        private Dictionary<string, LoadData> _pathDataMap;

        private bool _isBusy;

        private Queue<LoadData> _queue;

        private int versionBase;

        private Dictionary<LoadData, Promise<LoadData>> _promisesMap;

        private Dictionary<SceneLayer, Promise<SceneLayer>> _clearPromisesMap;

        [Inject] public IBundleInfoModel bundleInfoModel { get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
            _promisesMap = new Dictionary<LoadData, Promise<LoadData>>();
            _clearPromisesMap = new Dictionary<SceneLayer, Promise<SceneLayer>>();
            _layerDataMap = new Dictionary<SceneLayer, LoadData>();
            _pathDataMap = new Dictionary<string, LoadData>();
            _queue = new Queue<LoadData>();
            versionBase = int.Parse(Application.version.Replace(".", ""));
        }

        public IPromise<LoadData> LoadScene(string name, SceneLayer layer, bool reload = false)
        {
            return LoadScene(name, bundleInfoModel.GetPath(name), layer, reload);
        }

        public IPromise<LoadData> LoadScene(string name, string path, SceneLayer layer, bool reload = false)
        {
            Promise<LoadData> promise = new Promise<LoadData>();

            var loadData = new LoadData
            {
                Layer = layer,
                Name = name,
                Path = path
            };

            if (_pathDataMap.ContainsKey(loadData.Path))
            {
                Clear(layer).Then(sceneLayer =>
                {
                    if (!_isBusy)
                    {
                        _isBusy = true;
                        LoadBundleInner(loadData);
                    }
                    else
                    {
                        Debug.Log("Queued " + loadData.Name);
                        _queue.Enqueue(loadData);
                    }
                });
            }
            else
            {
                if (!_isBusy)
                {
                    _isBusy = true;
                    LoadBundleInner(loadData);
                }
                else
                {
                    Debug.Log("Queued " + loadData.Name);
                    _queue.Enqueue(loadData);
                }
            }

            _promisesMap.Add(loadData, promise);

            return promise;
        }

        private void LoadBundleInner(LoadData data)
        {
            if (_pathDataMap.ContainsKey(data.Path))
                return;

            MonoBehaviour root = contextView.GetComponent<ContextView>();
            try
            {
                root.StartCoroutine(LoadBundleWithUwr(data));
            }
            catch (Exception exception)
            {
                OnReject(data, exception);
                Debug.LogWarning(data.Name + " scene loading problem. " + exception.Message);
                CheckQueue();
            }
        }

        private void CheckQueue()
        {
            _isBusy = false;
            if (_queue.Count > 0)
            {
                Debug.Log("Load from queue");
                var loadData = _queue.Dequeue();
                LoadBundleInner(loadData);
            }
        }

        private IEnumerator LoadBundleWithUwr(LoadData data)
        {
            yield return ClearInner(data.Layer);

            Add(data);

            while (!Caching.ready)
                yield return null;
#if UNITY_EDITOR
            SceneManager.LoadScene(FirstCharToUpper(data.SceneName), LoadSceneMode.Additive);
#else
            using (UnityWebRequest www = new UnityWebRequest(data.Path))
            {
                DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, (uint) versionBase, 0);
                www.downloadHandler = handler;
                yield return www.SendWebRequest();

                if (www.error != null)
                {
                    OnReject(data, new Exception(www.error + " Data : " + data.Path));
                    yield break;
                }

                data.Bundle = handler.assetBundle;


                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(data.SceneName, LoadSceneMode.Additive);
    
                if (loadOperation != null)
                {
                    while (!loadOperation.isDone)
                        yield return null;
                }
        }
#endif

            OnComplated(data);

//            if (onCompleted != null)
//                onCompleted.Invoke(data.Layer);

            CheckQueue();
        }

        public string FirstCharToUpper(string s)
        {
// Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

// Return char and concat substring.  
            return char.ToUpper(s
                       [0]) + s.Substring(1);
        }

        private void Add(LoadData data)
        {
//      if (_layerDataMap.ContainsKey(data.Layer))
//      {
//        LoadData oldData = _layerDataMap[data.Layer];
//        _layerDataMap.Remove(data.Layer);
//        _pathDataMap.Remove(oldData.Path);
//        if (data.Bundle != null)
//          data.Bundle.Unload(true);
//        data.Dispose();
//      }
            _layerDataMap.Add(data.Layer, data);
            _pathDataMap.Add(data.Path, data);
            data.SceneName = FirstLetterToUpper(data.Name);
        }

        public IPromise<SceneLayer> Clear(SceneLayer layer)
        {
            Promise<SceneLayer> promise = new Promise<SceneLayer>();
            _clearPromisesMap.Add(layer, promise);
            MonoBehaviour root = contextView.GetComponent<ContextView>();
            root.StartCoroutine(ClearInner(layer));
            return promise;
        }

        public void OnComplated(LoadData data)
        {
            if (!_promisesMap.ContainsKey(data))
            {
                Debug.LogWarning("!_promisesMap.ContainsKey(data)");
                return;
            }

            _promisesMap[data].Resolve(data);
            _promisesMap.Remove(data);
        }

        public void OnClear(SceneLayer data)
        {
            if (!_clearPromisesMap.ContainsKey(data))
            {
//                Debug.LogWarning("!_clearPromisesMap.ContainsKey(data)");
                return;
            }

            var clearPromises = _clearPromisesMap[data];
            if (clearPromises.CurState == PromiseState.Pending
            )
            {
                clearPromises.Resolve(data);
            }

            _clearPromisesMap.Remove(data);
        }

        public void OnReject(LoadData data, Exception exception)
        {
            if (!_promisesMap.ContainsKey(data))
            {
                Debug.LogWarning("!_promisesMap.ContainsKey(data)");
                return;
            }

            Debug.Log(exception.Message);
            _promisesMap[data].Reject(exception);
            _promisesMap.Remove(data);
        }

        private IEnumerator ClearInner(SceneLayer layer)
        {
            if (!_layerDataMap.ContainsKey(layer))
            {
                OnClear(layer);
                yield break;
            }

            var data = _layerDataMap[layer];
            _layerDataMap.Remove(layer);
            if (data == null)
            {
                OnClear(layer);
                yield break;
            }

            var sceneByName = SceneManager.GetSceneByName(data.SceneName);
            if (sceneByName.isLoaded)
            {
                AsyncOperation operation = SceneManager.UnloadSceneAsync(data.SceneName);
                if (operation != null)
                {
//        Debug.Log("Unloading...");
                    while (!operation.isDone)
                        yield return null;
                }
            }

            _pathDataMap.Remove(data.Path);
            if (data.Bundle != null)
                data.Bundle.Unload(true);
            OnClear(layer);
        }

        public void ClearLayers(SceneLayer[] layers)
        {
            foreach (SceneLayer layer in layers)
            {
                Clear(layer);
            }
        }

        private string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;
            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);
            return str.ToUpper();
        }
    }

    public class LoadData
    {
        public string Name { get; set; }
        public string SceneName { get; set; }
        public string Path { get; set; }
        public SceneLayer Layer { get; set; }
        public AssetBundle Bundle { get; set; }
    }
}