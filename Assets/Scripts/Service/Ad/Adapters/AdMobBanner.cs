using System;
using GoogleMobileAds.Api;
using Service.Ad.Enums;
using Service.Ad.Interfaces;
using UnityEngine;

namespace Service.Ad.Adapters
{
    public class AdMobBanner : IAdAdapter
    {
        public int Duration { get; set; }

        private bool _isLoaded;

        private bool _isInited;

        private bool _show;

        private BannerView _ad;

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
                return true;
            }
        }

        public int RemainingTime
        {
            get
            {
                return 0;
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

            _ad = new BannerView(ZoneId, AdSize.SmartBanner, AdPosition.Top);

            _ad.OnAdLoaded += HandleOnBannerLoaded;
            _ad.OnAdFailedToLoad += HandleOnBannerLoadFail;
            _ad.OnAdClosed += HandleOnBannerClosed;

            _request = new AdRequest.Builder().Build();

            Load();

            _isInited = true;
        }

        public void Load()
        {
            Log("Load");
            if (IsLoaded)
                return;

            _ad.LoadAd(_request);
        }

        public bool Show(bool time)
        {
            Log("Show");

            if (!IsLoaded)
            {
                _show = true;
                Load();
                return false;
            }

            _adservice.Dispatcher.Dispatch(AdEvent.AdOpening);

            _ad.Show();

            _show = false;

            return true;
        }

        public void Hide()
        {
            Log("Hide");
            _show = false;
            _ad.Hide();
        }

        private void HandleOnBannerLoaded(object sender, EventArgs e)
        {
            Log("HandleOnBannerLoaded");
            _isLoaded = true;
            if (_show)
                Show(false);
            else
            {
                Hide();
            }
        }

        private void HandleOnBannerLoadFail(object sender, AdFailedToLoadEventArgs e)
        {
            Log("HandleOnBannerLoadFail");

            _isLoaded = false;
        }

        private void HandleOnBannerClosed(object sender, EventArgs e)
        {
            Log("HandleOnBannerClosed");

            _isLoaded = false;
            _adservice.Close(_zone);
        }

        private void Log(string message)
        {
            if (_adservice.DebugMode)
                Debug.LogWarning("AdMobBanner > " + message);
        }

        public int RewardAmount { get; set; }

        public void Destroy()
        {
            _ad.Destroy();
            _adservice = null;
        }
    }
}