using Core.Manager.Screen;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.View
{
    public class RateUsPanelView : CoreView,IPanelView
    {
        public int Rate { get; private set; }

        public void OnRateClick(int rate)
        {
            Rate = rate;
            DispatchDelayed(RateUsPanelEvent.Rate);
        }

        [UsedImplicitly]
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                DispatchDelayed(RateUsPanelEvent.Cancel);
        }

        public IPanelVo vo { get; set; }
    }
}
