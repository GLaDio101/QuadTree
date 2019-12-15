using System;
using System.Collections;
using System.Collections.Generic;
using Core.Manager.Audio;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using Service.Ad.Enums;
using Service.Ad.Interfaces;
using Service.Ad.Vo;
using Service.NetConnection;
using Service.Tracking;
using UnityEngine;

namespace Service.Ad
{
    public class SuperAdService : IAdService, IAdServiceInternal
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject RootObject { get; set; }

        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        public IEventDispatcher Dispatcher
        {
            get { return dispatcher; }
        }

        [Inject]
        public ITrackingService trackingService { get; set; }

        public ITrackingService TrackingService
        {
            get { return trackingService; }
        }

        [Inject]
        public INetConnectionService netConnectionService { get; set; }

        private bool _debugMode;

        public bool DebugMode
        {
            get { return _debugMode; }
        }

        public int CurrentRewardAmount { get; set; }

        private bool _closed;

        private string _closedZone;

        private IDictionary<string, AdEntry> _adapterMap;

        private List<AdServiceVo> _adServiceList;

        public void Load(AdServiceData adServiceData)
        {
            _adServiceList = new List<AdServiceVo>();
//            Debug.Log(JsonConvert.SerializeObject(adServiceData));
            _adServiceList = adServiceData.AdServiceList;
        }

        public List<AdServiceVo> GetAdServiceList()
        {
            return _adServiceList;
        }

        public int GetTime()
        {
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        }

        public void Init(IList<AdEntry> list)
        {
#if DEBUG
            _debugMode = false;
#else
            _debugMode = false;
#endif
            if (_debugMode)
                Debug.LogWarning("Initializing Ad Service...");


            dispatcher.AddListener(NetConnectionEvent.StatusChanged, OnInternetStatusChanged);

            _adapterMap = new Dictionary<string, AdEntry>();
            foreach (AdEntry adEntry in list)
            {
                _adapterMap.Add(adEntry.Zone, adEntry);
            }

            OnInternetStatusChanged();
        }

        private void OnInternetStatusChanged()
        {
            if (netConnectionService.Status != NetConnectionStatus.Reachable) return;

            if (_debugMode)
                Debug.LogWarning("Initializing Ad Adapters...");

            foreach (KeyValuePair<string, AdEntry> keyValuePair in _adapterMap)
            {
                keyValuePair.Value.Adapter.Init(this, keyValuePair.Key);
            }
        }

        public void Reset()
        {
            if (_debugMode)
                Debug.LogWarning("Reset Ad Service");

            dispatcher.RemoveListener(NetConnectionEvent.StatusChanged, OnInternetStatusChanged);

            foreach (KeyValuePair<string, AdEntry> keyValuePair in _adapterMap)
            {
                keyValuePair.Value.Adapter.Destroy();
            }

            _adapterMap.Clear();
        }

        public void CheckClosedThreadSafely()
        {
            _closed = false;
            MonoBehaviour root = RootObject.GetComponent<ContextView>();
            root.StartCoroutine(OnCheckIsClosed());
        }

        private IEnumerator OnCheckIsClosed()
        {
            while (!_closed)
            {
                yield return null;
            }

            _closed = false;

            foreach (KeyValuePair<string, AdEntry> keyValuePair in _adapterMap)
            {
                keyValuePair.Value.Adapter.Load();
            }

            Dispatcher.Dispatch(AdEvent.AdClosed, _closedZone);
            Dispatcher.Dispatch(AudioEvent.UnMute);
            Dispatcher.Dispatch(AudioEvent.StartGameplay);
            _closedZone = String.Empty;
        }

        public void Close(string zone)
        {
            _closedZone = zone;
            _closed = true;
        }

        public bool Show(string zone, AdShowType type)
        {
            if (netConnectionService.Status != NetConnectionStatus.Reachable)
            {
                if (_debugMode)
                    Debug.LogWarning("No connections are available.");
                return false;
            }

            if (!_adapterMap.ContainsKey(zone))
            {
                if (_debugMode)
                    Debug.LogWarning("Undefined zone: " + zone);
                return false;
            }

            CurrentRewardAmount = 0;
            Dispatcher.Dispatch(AudioEvent.Mute);

            switch (type)
            {
                case AdShowType.Now:
                    return _adapterMap[zone].Adapter.Show(false);
                case AdShowType.Timer:
                    {
                        if (_adapterMap[zone].Adapter.IsTimeUp)
                            return _adapterMap[zone].Adapter.Show(true);
                        return false;
                    }
                case AdShowType.Counter:
                    {
                        _adapterMap[zone].CurrentCount++;
                        if (_adapterMap[zone].Adapter.Count == _adapterMap[zone].CurrentCount)
                        {
                            _adapterMap[zone].CurrentCount = 0;
                            _adapterMap[zone].Adapter.Show(true);
                            return true;
                        }

                        return false;
                    }
            }
            return false;
        }

        public void Hide(string zone)
        {
            if (netConnectionService.Status != NetConnectionStatus.Reachable)
            {
                if (_debugMode)
                    Debug.LogWarning("No connections are available.");
                return;
            }

            if (!_adapterMap.ContainsKey(zone))
            {
                if (_debugMode)
                    Debug.LogWarning("Undefined zone: " + zone);
                return;
            }

            _adapterMap[zone].Adapter.Hide();
        }
    }
}