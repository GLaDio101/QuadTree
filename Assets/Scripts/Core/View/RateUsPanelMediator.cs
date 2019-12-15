using Core.Events;
using Core.Manager.Screen;
using Core.Model;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using Service.Tracking;
using UnityEngine;

namespace Core.View
{
    public enum RateUsPanelEvent
    {
        Rate,
        Cancel
    }

    public class RateUsPanelMediator : EventMediator
    {
        [Inject]
        public RateUsPanelView view { get; set; }

        [Inject]
        public IBasePlayerModel playerModel { get; set; }

        [Inject]
        public ITrackingService trackingService { get; set; }

        private PanelVo vo
        {
            get { return view.vo as PanelVo; }
        }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(RateUsPanelEvent.Rate, OnRate);
            view.dispatcher.AddListener(RateUsPanelEvent.Cancel, OnCancel);
        }

        private void OnRate(IEvent payload)
        {
            trackingService.Event("Rate", view.Rate.ToString());
            //playerModel.Settings.ShowRateUs = false;
            dispatcher.Dispatch(BaseGameEvent.SaveGame);
            if (view.Rate > 3)
            {
                Application.OpenURL("market://details?id=" + Application.identifier);
                // add for ios
            }
            if (vo.OnCancel != null)
                vo.OnCancel();
            Destroy(gameObject);
        }

        private void OnCancel(IEvent payload)
        {
            if (vo.OnCancel != null)
                vo.OnCancel();
            Destroy(gameObject);
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(RateUsPanelEvent.Rate, OnRate);
            view.dispatcher.RemoveListener(RateUsPanelEvent.Cancel, OnCancel);
        }
    }
}