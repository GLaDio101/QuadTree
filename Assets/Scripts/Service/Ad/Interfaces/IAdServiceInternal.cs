using strange.extensions.dispatcher.eventdispatcher.api;
using Service.Tracking;
using UnityEngine;

namespace Service.Ad.Interfaces
{
    public interface IAdServiceInternal
    {
        IEventDispatcher Dispatcher { get; }

        ITrackingService TrackingService { get; }

        void CheckClosedThreadSafely();

        void Close(string zone);

        int GetTime();

        int CurrentRewardAmount { set; }

        bool DebugMode { get; }

        GameObject RootObject { get; set; }
    }
}