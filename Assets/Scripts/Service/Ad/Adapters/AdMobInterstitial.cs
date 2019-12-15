using System;
using GoogleMobileAds.Api;
using Service.Ad.Enums;
using Service.Ad.Interfaces;
using UnityEngine;

namespace Service.Ad.Adapters
{
  public class AdMobInterstitial : IAdAdapter
  {
    public int Duration { get; set; }

    private int _lastTime;

    private bool _isLoaded;

    private bool _isInited;

    private InterstitialAd _ad;

    private AdRequest _request;

    private IAdServiceInternal _adservice;

    public bool IsLoaded
    {
      get
      {
        if (!_isLoaded)
          Log("Not Loaded.");
        return _isLoaded;
      }
    }

    public bool IsTimeUp
    {
      get
      {
        if (_lastTime == 0)
          return true;
        var time = _adservice.GetTime() - _lastTime;
        Log("Remaining Time: " + RemainingTime);
        return time > Duration;
      }
    }

    public int RemainingTime
    {
      get
      {
        if (_adservice == null)
          return 0;

        var time = _adservice.GetTime() - _lastTime;
        time = Duration - time;
        return time > 0 ? time : 0;
      }
    }

    public int Count { get; set; }

    public string ZoneId { get; set; }

    private string _zone;

    public void Init(IAdServiceInternal adservice, string zone)
    {
      if (_isInited)
        return;

      _adservice = adservice;
      _zone = zone;

      Log("Init");

      Load();

      _isInited = true;
    }

    public void Load()
    {
      Log("Load");
      if (IsLoaded)
        return;

      _ad = new InterstitialAd(ZoneId);

      _ad.OnAdLoaded += HandleOnInterstitialLoaded;
      _ad.OnAdFailedToLoad += HandleOnInterstitialLoadFail;
      _ad.OnAdClosed += HandleOnInterstitialClosed;

      _request = new AdRequest.Builder().Build();

      StartInterstitialTimer();

      _ad.LoadAd(_request);
    }

    public bool Show(bool time)
    {
      Log("Show");

      if (!IsLoaded)
      {
        Load();
        return false;
      }

      _adservice.Dispatcher.Dispatch(AdEvent.AdOpening);

      _ad.Show();

      _adservice.CheckClosedThreadSafely();

      StartInterstitialTimer();

      return true;
    }

    private void StartInterstitialTimer()
    {
      Log("StartInterstitialTimer");
      _lastTime = _adservice.GetTime();
    }

    private void HandleOnInterstitialLoaded(object sender, EventArgs e)
    {
      Log("HandleOnInterstitialLoaded");
      _isLoaded = true;
    }

    private void HandleOnInterstitialLoadFail(object sender, AdFailedToLoadEventArgs e)
    {
      Log("HandleOnInterstitialLoadFail" + e.Message);

      _isLoaded = false;
      StartInterstitialTimer();
      _adservice.Close(_zone);
    }

    private void HandleOnInterstitialClosed(object sender, EventArgs e)
    {
      Log("HandleOnInterstitialClosed");

      _isLoaded = false;
      StartInterstitialTimer();
      _adservice.Close(_zone);
    }

    private void Log(string message)
    {
      if (_adservice.DebugMode)
        Debug.LogWarning("AdMobInterstitial > " + message);
    }

    public void Hide()
    {
      throw new NotImplementedException();
    }

    public int RewardAmount { get; set; }

    public void Destroy()
    {
      _ad.Destroy();
      _adservice = null;
    }
  }
}