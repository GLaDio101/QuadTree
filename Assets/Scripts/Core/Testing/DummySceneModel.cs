#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using Core.Manager.Scene;
using Core.Promise;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Testing
{
  public class DummySceneModel : ISceneModel
  {
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    private Dictionary<SceneLayer, LoadData> _layerDataMap;

    private Dictionary<string, LoadData> _pathDataMap;

    private bool _isBusy;

    private Queue<LoadData> _queue;

    private Dictionary<LoadData, Promise<LoadData>> _promisesMap;

    private Dictionary<SceneLayer, Promise<SceneLayer>> _clearPromisesMap;

    [PostConstruct]
    public void OnPostConstruct()
    {
      _promisesMap = new Dictionary<LoadData, Promise<LoadData>>();
      _clearPromisesMap = new Dictionary<SceneLayer, Promise<SceneLayer>>();
      _layerDataMap = new Dictionary<SceneLayer, LoadData>();
      _pathDataMap = new Dictionary<string, LoadData>();
      _queue = new Queue<LoadData>();
    }

    public IPromise<LoadData> LoadScene(string name, SceneLayer layer, bool reload = false)
    {
      Promise<LoadData> promise = new Promise<LoadData>();

      var loadData = new LoadData
      {
        Layer = layer,
        Name = name,
        Path = "Assets/Scenes/" + FirstLetterToUpper(name) + ".unity",
      };
      _promisesMap.Add(loadData, promise);

      if (_pathDataMap.ContainsKey(loadData.Path))
      {
        Reload(layer);
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

      return promise;
    }

    public IPromise<LoadData> LoadScene(string name, string path, SceneLayer layer, bool reload = false)
    {
      return null;
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


//      if (onCompleted != null)
//        onCompleted.Invoke(data.Layer);
      OnComplated(data);
      CheckQueue();
    }

    private void Add(LoadData data)
    {
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

    private IEnumerator ClearInner(SceneLayer layer)
    {
      if (!_layerDataMap.ContainsKey(layer))
      {
        OnClear(layer);

//                if (onCompleted != null)
//                    onCompleted.Invoke(layer);
        yield break;
      }

      var data = _layerDataMap[layer];
      _layerDataMap.Remove(layer);
      if (data == null)
      {
        OnClear(layer);

//                if (onCompleted != null)
//                    onCompleted.Invoke(layer);
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

      OnClear(layer);
//            if (onCompleted != null)
//                onCompleted.Invoke(layer);
    }

    public void OnComplated(LoadData data)
    {
      _promisesMap[data].Resolve(data);
      _promisesMap.Remove(data);
    }

    public void OnReject(LoadData data, Exception exception)
    {
      _promisesMap[data].Reject(exception);
      _promisesMap.Remove(data);
    }

    public void OnClear(SceneLayer data)
    {
      if (!_clearPromisesMap.ContainsKey(data))
        return;

      _clearPromisesMap[data].Resolve(data);
      _clearPromisesMap.Remove(data);
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

    public void Reload(SceneLayer layer)
    {
      if (!_layerDataMap.ContainsKey(layer))
        return;

      var data = _layerDataMap[layer];

      Clear(layer);
    }
  }
}
#endif