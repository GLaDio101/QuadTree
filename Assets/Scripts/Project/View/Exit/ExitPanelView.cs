using Core.Manager.Screen;
using Core.View;
using JetBrains.Annotations;
using Project.View.Confirm;
using UnityEngine;

namespace Project.View.Exit
{
    public class ExitPanelView : CoreView, IPanelView
    {
        public void OnCancelClick()
        {
            DispatchDelayed(ExitPanelEvent.Cancel);
        }

        public void OnChangeUserClick()
        {
            DispatchDelayed(ExitPanelEvent.ChangeUser);
        }

        public void OnExitClick()
        {
            DispatchDelayed(ExitPanelEvent.Exit);
        }

        [UsedImplicitly]
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !vo.NotCancellable)
                DispatchDelayed(ConfirmPanelEvent.Cancel);
        }

        public IPanelVo vo { get; set; }
    }
}
