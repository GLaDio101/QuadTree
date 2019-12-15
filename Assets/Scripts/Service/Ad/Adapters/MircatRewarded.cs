/*using System;
using System.Diagnostics;
using Service.Ad.Enums;
using Service.Ad.Interfaces;
using Service.Ad.Player;
using Service.Ad.Utils;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Service.Ad.Adapters
{
    public class MircatRewarded : IAdAdapter
    {
        public int Duration { get; set; }

        public string ClickLink { get; set; }

        public string ImpressionLink { get; set; }

        public int SkipButtonDuration { get; set; }

        public string VideoId { get; set; }

        private int _lastTime;

        private bool _isInited;

        private IAdServiceInternal _adservice;

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

        private IAdVideoPlayer _player;

        public void Init(IAdServiceInternal adservice, string zone)
        {
            if (_isInited)
                return;

            _adservice = adservice;
            _zone = zone;
            _isLoaded = false;

            _player = YoutubePlayer.GetInstance(_adservice.RootObject.transform);

            if (_player == null)
            {
                Debug.LogError("Ad Video player not found please add one.");
                return;
            }

            Debug.Log("init");
            _player.OnSkip += OnSkipped;
            _player.OnCompleted += OnCompleted;
            _player.OnClick += OnClick;

            Log("Init");

            Load();

            _isInited = true;
        }

        public void Load()
        {
            if (_player == null)
            {
                Debug.LogError("Ad Video player not found please add one.");
                return;
            }

            if (_isLoaded)
                return;

            _player.Load(VideoId);
            _isLoaded = true;
        }

        public bool Show(bool time)
        {
            if (_player == null)
            {
                Debug.LogError("Ad video player not found please add one.");
                return false;
            }

            Log("Show");
            _adservice.Dispatcher.Dispatch(AdEvent.AdOpening);

            try
            {
                _player.Camera = Camera.main;
                _player.Play();
                _player.ShowSkip(SkipButtonDuration);

                if (time)
                    _lastTime = _adservice.GetTime();

                _adservice.CheckClosedThreadSafely();

                _adservice.TrackingService.Event("Ad", "Rewarded_Show");

                WWW www = new WWW(ImpressionLink);
                www.Dispose();

                return true;
            }
            catch (Exception)
            {
                OnFailed();
            }

            return false;
        }

        private void OnClick()
        {
            Application.OpenURL(ClickLink);
        }

        private void OnSkipped()
        {
            Log("Skipped");

            _adservice.TrackingService.Event("Ad", "Rewarded_Skipped");
            _adservice.CurrentRewardAmount = RewardAmount;
            _isLoaded = false;
            _adservice.Close(_zone);
        }

        private void OnFailed()
        {
            Log("Failed");
            _adservice.TrackingService.Event("Ad", "Rewarded_Fail");
            _isLoaded = false;
            _adservice.Close(_zone);
        }

        private void OnCompleted()
        {
            Log("Rewarded");
            _adservice.TrackingService.Event("Ad", "Rewarded_Success");
            _adservice.CurrentRewardAmount = RewardAmount;
            _isLoaded = false;
            _adservice.Close(_zone);
        }

        [Conditional("DEVELOPMENT_BUILD")]
        private void Log(string message)
        {
            if (_adservice.DebugMode)
                Debug.LogWarning("MircatRewarded > " + message);
        }

        public void Hide()
        {

        }

        public int RewardAmount { get; set; }

        public void Destroy()
        {
            _adservice = null;
            _player = null;
        }
    }
}*/