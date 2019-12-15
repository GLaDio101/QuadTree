/*using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.RemoteConfig;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Assets.Scripts.Service.Config.Imp
{
    public class FirebaseRemoteConfigService : IConfigService
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        //[PostConstruct]
        //public void OnPostConstruct()
        //{
        //    var configSettings = new ConfigSettings { IsDeveloperMode = true };
        //    FirebaseRemoteConfig.Settings = configSettings;
        //}

        public string GetStringValue(string key)
        {
            return FirebaseRemoteConfig.GetValue(key).StringValue;
        }

        public int GetIntValue(string key)
        {
            return (int)FirebaseRemoteConfig.GetValue(key).LongValue;
        }

        public bool GetBoolValue(string key)
        {
            return FirebaseRemoteConfig.GetValue(key).BooleanValue;
        }

        public float GetFloatValue(string key)
        {
            return (float)FirebaseRemoteConfig.GetValue(key).DoubleValue;
        }

        public void Load()
        {
            var task = FirebaseRemoteConfig.FetchAsync(TimeSpan.FromMilliseconds(1));
            task.ContinueWith(FetchComplete);
        }

        private void FetchComplete(Task task)
        {
            switch (FirebaseRemoteConfig.Info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.ActivateFetched();
                    break;
                case LastFetchStatus.Failure:
                    switch (FirebaseRemoteConfig.Info.LastFetchFailureReason)
                    {
                        case FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " +
                                     FirebaseRemoteConfig.Info.ThrottledEndTime);
                            break;
                    }
                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }

            dispatcher.Dispatch(ConfigEvent.DataReady);
        }

        public void SetDefaults(Dictionary<string, object> list)
        {
            FirebaseRemoteConfig.SetDefaults(list);
        }
    }
}*/