using System;
using System.Collections;
using System.Collections.Generic;
using Core.Promise;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Manager.Bundle
{
  public class BundleModel : IBundleModel
  {
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    protected Dictionary<string, BundleLoadData> _nameDataMap;

    protected Dictionary<string, BundleLoadData> _pathDataMap;

    protected bool _isBusy;

    protected Queue<BundleLoadData> _queue;

    protected int versionBase;

    protected Dictionary<BundleLoadData, Promise<BundleLoadData>> _promisesMap;

    protected MonoBehaviour root;

    [Inject]
    public IBundleInfoModel bundleInfoModel { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      Caching.ClearCache();
      _pathDataMap = new Dictionary<string, BundleLoadData>();
      _nameDataMap = new Dictionary<string, BundleLoadData>();
      _promisesMap = new Dictionary<BundleLoadData, Promise<BundleLoadData>>();
      _queue = new Queue<BundleLoadData>();
      versionBase = int.Parse(Application.version.Replace(".", ""));
      root = contextView.GetComponent<ContextView>();
    }

    public BundleLoadData GetBundleByName(string name)
    {
      if (!_nameDataMap.ContainsKey(name))
      {
        Debug.LogWarning("Bundle not found by name :" + name);
        return null;
      }

      return _nameDataMap[name];
    }


    public IPromise<BundleLoadData> LoadBundle(string name, bool load = false)
    {
      return LoadBundle(name,bundleInfoModel.GetPath(name),load);
    }

    public IPromise<BundleLoadData> LoadBundle(string name, string path, bool load = false)
    {
      var loadData = new BundleLoadData
      {
        Name = name,
        Load = load,
        Path = path
      };

      Promise<BundleLoadData> promise = new Promise<BundleLoadData>();

      _promisesMap.Add(loadData, promise);


      if (_pathDataMap.ContainsKey(loadData.Path))
      {
        var predata = _pathDataMap[loadData.Path];
//        Debug.Log(loadData.Path + " already loaded.");
        promise.Resolve(predata);
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
//          Debug.Log("Queued " + loadData.Name);
          _queue.Enqueue(loadData);
        }
      }

      return promise;
    }

    protected void LoadBundleInner(BundleLoadData data)
    {
      if (_pathDataMap.ContainsKey(data.Path))
      {
        _promisesMap[data].Reject(new BundleAlreadyLoadedException(data));
        return;
      }


      _isBusy = true;
      root.StartCoroutine(LoadBundleWithUrl(data));
    }

    protected void CheckQueue()
    {
      _isBusy = false;
      if (_queue.Count > 0)
      {
        LoadBundleInner(_queue.Dequeue());
      }
    }

    protected IEnumerator LoadBundleWithUrl(BundleLoadData data)
    {
      while (!Caching.ready)
        yield return null;

//      Debug.Log(data.Path);
      using (UnityWebRequest www = new UnityWebRequest(data.Path))
      {
        DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, (uint) versionBase, 0);
        www.downloadHandler = handler;
        yield return www.SendWebRequest();

        if (www.error != null)
        {
          _promisesMap[data].Reject(new BundleNetworkException(www.error, data));
          Debug.LogWarning(data.Name + " bundle loading problem. " + www.error + " " + data.Path);
          CheckQueue();
          yield break;
        }

        data.Bundle = handler.assetBundle;

        Clear(data.Name);
        _pathDataMap.Add(data.Path, data);
        _nameDataMap.Add(data.Name, data);
        if (data.Load && data.Bundle != null)
        {
          AsyncOperation loadOperation = data.Bundle.LoadAllAssetsAsync();

          if (loadOperation != null)
          {
            while (!loadOperation.isDone)
              yield return null;
          }
        }

        CheckQueue();

        //Load Success
        _promisesMap[data].Resolve(data);
      }
    }

    public void Clear(string name, bool clearAll = true)
    {
      if (!_nameDataMap.ContainsKey(name))
        return;

      var data = _nameDataMap[name];


      if (data == null)
        return;

      _pathDataMap.Remove(data.Path);
      _nameDataMap.Remove(data.Name);

      if (data.Bundle != null)
        data.Bundle.Unload(clearAll);

      data.Dispose();
    }

    public void ClearLayers(string[] names)
    {
      foreach (string t in names)
      {
        Clear(t);
      }
    }
  }

  public class BundleLoadData : IDisposable
  {
    public string Name { get; set; }

    public string Path { get; set; }

    public AssetBundle Bundle { get; set; }

    public bool Load { get; set; }

    public void Dispose()
    {
      Bundle = null;
    }
  }
}