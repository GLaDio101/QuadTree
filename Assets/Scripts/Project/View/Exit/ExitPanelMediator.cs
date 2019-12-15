using Project.Enums.Events;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace Project.View.Exit
{
    public enum ExitPanelEvent
    {
        Cancel,
        Exit,
        ChangeUser
    }

    public class ExitPanelMediator : EventMediator
    {
        [Inject]
        public ExitPanelView view { get; set; }

        private ExitPanelVo vo
        {
            get { return vo as ExitPanelVo; }
        }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(ExitPanelEvent.Exit, OnExit);
            view.dispatcher.AddListener(ExitPanelEvent.ChangeUser, OnChangeUser);
            view.dispatcher.AddListener(ExitPanelEvent.Cancel, OnCancel);
        }

        private void OnExit(IEvent payload)
        {
            dispatcher.Dispatch(GamePlayEvent.Quit);
            Destroy(gameObject);
        }

        private void OnChangeUser(IEvent payload)
        {
            dispatcher.Dispatch(GamePlayEvent.Logout);
            Destroy(gameObject);
        }

        private void OnCancel(IEvent payload)
        {
            Destroy(gameObject);
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(ExitPanelEvent.Exit, OnExit);
            view.dispatcher.RemoveListener(ExitPanelEvent.ChangeUser, OnChangeUser);
            view.dispatcher.RemoveListener(ExitPanelEvent.Cancel, OnCancel);
        }
    }
}