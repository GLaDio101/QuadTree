using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using YoutubeLight;

public class HandheldPlayback : MonoBehaviour
{
  private RequestResolver _resolver;

#if UNITY_IPHONE || UNITY_ANDROID
    private string _decryptedUrl;
#endif
  private bool _completed;

  public bool Completed
  {
    get { return _completed; }
  }

  [UsedImplicitly]
  private void Start()
  {
    _resolver = gameObject.AddComponent<RequestResolver>();
  }

  public void Load(string url)
  {
    StartCoroutine(_resolver.GetDownloadUrls(FinishLoadingUrls, url, false));
  }

  private void FinishLoadingUrls()
  {
    List<VideoInfo> videoInfos = _resolver.videoInfos;
    foreach (VideoInfo info in videoInfos)
    {
      if (info.VideoType == VideoType.Mp4 && info.Resolution == (720))
      {
        if (info.RequiresDecryption)
        {
          //The string is the video url
          StartCoroutine(_resolver.DecryptDownloadUrl(DecryptionFinished, info));
          break;
        }

        DecryptionFinished(info.DownloadUrl);
        break;
      }
    }
  }

  private void DecryptionFinished(string url)
  {
#if UNITY_IPHONE || UNITY_ANDROID
        _decryptedUrl = url;
    #endif
  }

  public void Play()
  {
    _completed = false;
    StartCoroutine(PlayInner());
  }

  public IEnumerator PlayInner()
  {
#if UNITY_IPHONE || UNITY_ANDROID
        Handheld.PlayFullScreenMovie(_decryptedUrl, Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);
#else
    Debug.Log("This only runs in mobile");
#endif
    yield return new WaitForEndOfFrame();
    yield return new WaitForEndOfFrame();

    _completed = true;
  }
}