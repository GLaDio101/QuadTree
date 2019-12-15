using System;
using System.Diagnostics;
using System.Globalization;
using GoogleMobileAds.Api;
using Service.Ad.Enums;
using Service.Ad.Interfaces;
using Debug = UnityEngine.Debug;

namespace Service.Ad.Adapters
{
    public class AdMobRewarded : IAdAdapter
    {
        public int Duration { get; set; }

        private int _lastTime;

        private bool _isInited;

        private IAdServiceInternal _adservice;
        
        private RewardBasedVideoAd _ad;

        private AdRequest _request;
        
        private bool _isLoaded;

        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        public bool IsTimeUp
        {
            get
            {
                if (_lastTime == 0)
                    return true;
                var time = _adservice.GetTime() - _lastTime;
                Log("Remaining Time: " + (Duration - time));
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

            _ad = RewardBasedVideoAd.Instance;
            _ad.OnAdLoaded += OnRewardedVideoLoaded;
            _ad.OnAdClosed += OnRewardedVideoAdClosed;
            _ad.OnAdFailedToLoad += OnRewardedVideoAdFailedToLoad;
            _ad.OnAdRewarded += OnRewarded;
            _ad.OnAdLeavingApplication += OnRewardedVideoAdLeftApplication;
            _ad.OnAdOpening += OnRewardedVideoAdOpened;
            _ad.OnAdStarted += OnRewardedVideoStarted;
            
             _request = new AdRequest.Builder().Build();
            
            Load();

            _isInited = true;
        }

        public void Load()
        {
            if (IsLoaded)
                return;

            _ad.LoadAd(_request, ZoneId);
        }

        public bool Show(bool time)
        {
            if (IsLoaded)
            {
                Log("Show");
                _adservice.Dispatcher.Dispatch(AdEvent.AdOpening);

                if(time)
                    _lastTime = _adservice.GetTime();

                _adservice.CheckClosedThreadSafely();

                _ad.Show();
                return true;
            }

            return false;
        }
        
        private void OnRewarded(object sender, Reward e)
        {
            Log("Rewarded");
            _adservice.TrackingService.Event("Ad","AdRewarded_Success",e.Amount.ToString(CultureInfo.InvariantCulture));
            _adservice.CurrentRewardAmount = RewardAmount;
            CloseRewarded();
        }

        private void OnRewardedVideoAdClosed(object sender, EventArgs e)
        {
            Log("Skipped");
            CloseRewarded();
        }
        
        private void OnRewardedVideoAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            Log("Failed");
            CloseRewarded();
        }
        
        private void CloseRewarded()
        {
            _isLoaded = false;

            _adservice.Close(_zone);
        }

        private void OnRewardedVideoLoaded(object sender, EventArgs e)
        {
            Log("OnRewardedVideoLoaded");
            _isLoaded = true;
        }

        private void OnRewardedVideoStarted(object sender, EventArgs e)
        {
            Log("OnRewardedVideoStarted");
        }

        private void OnRewardedVideoAdOpened(object sender, EventArgs e)
        {
            Log("OnRewardedVideoAdOpened");
        }

        private void OnRewardedVideoAdLeftApplication(object sender, EventArgs e)
        {
            Log("OnRewardedVideoAdLeftApplication");
        }

        [Conditional("DEVELOPMENT_BUILD")]
        private void Log(string message)
        {
            if (_adservice.DebugMode)
                Debug.LogWarning("AdMobRewarded > " + message);
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }

        public int RewardAmount { get; set; }

        public void Destroy()
        {
            _adservice = null;
        }
    }
}