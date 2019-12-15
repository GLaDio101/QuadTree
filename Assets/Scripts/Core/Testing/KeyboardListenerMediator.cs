using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace Core.Testing
{
    public class KeyboardListenerMediator : EventMediator
    {
        [Inject]
        public KeyboardListenerView view { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(KeyboardEvent.KeyPressed,OnKeyPressed);
        }

        private void OnKeyPressed(IEvent payload)
        {
            dispatcher.Dispatch(payload.data);
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(KeyboardEvent.KeyPressed, OnKeyPressed);
        }
    }
}