using System.Diagnostics;
using Service.Ad.Enums;
using Service.Ad.Interfaces;
using UnityEngine.Advertisements;
using Debug = UnityEngine.Debug;

namespace Service.Ad.Adapters
{
    public class UnityRewarded : IAdAdapter
    {
        public int Duration { get; set; }

        public string GameId { get; set; }

        private int _lastTime;

        private bool _isInited;

        private IAdServiceInternal _adservice;

        public bool IsLoaded
        {
            get { return Advertisement.IsReady(); }
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

            Advertisement.Initialize(GameId);

            Load();

            _isInited = true;
        }

        public void Load()
        {
        }

        public bool Show(bool time)
        {
            Log("Show");
            if (Advertisement.IsReady(ZoneId))
            {
                _adservice.Dispatcher.Dispatch(AdEvent.AdOpening);

                var options = new ShowOptions { resultCallback = HandleShowResult };

                Advertisement.Show(ZoneId, options);

                if (time)
                    _lastTime = _adservice.GetTime();

                _adservice.CheckClosedThreadSafely();

                _adservice.TrackingService.Event("Ad", "Rewarded_Show");

                return true;
            }

            return false;
        }

        private void HandleShowResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Log("Rewarded");
                    _adservice.TrackingService.Event("Ad", "Rewarded_Success");
                    _adservice.CurrentRewardAmount = RewardAmount;
                    break;
                case ShowResult.Skipped:
                    Log("Skipped");
                    _adservice.TrackingService.Event("Ad", "Rewarded_Skipped");
                    break;
                case ShowResult.Failed:
                    Log("Failed");
                    _adservice.TrackingService.Event("Ad", "Rewarded_Fail");
                    break;
            }

            _adservice.Close(_zone);
        }

        [Conditional("DEVELOPMENT_BUILD")]
        private void Log(string message)
        {
            if (_adservice.DebugMode)
                Debug.LogWarning("UnityRewarded > " + message);
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }

        public int RewardAmount { get; set; }

        public void Destroy()
        {
            _adservice = null;
        }
    }
}