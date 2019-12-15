using Core.Enums;
using Core.Manager.Scene;
using Core.Manager.Screen;
using strange.extensions.mediation.impl;

namespace Core.View
{
    public enum CreditsScreenEvent
    {
        Skip,
        OtherGames
    }

    public class CreditsScreenMediator : EventMediator
    {
        [Inject]
        public CreditsScreenView view { get; set; }

        [Inject]
        public ISceneModel sceneModel { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(CreditsScreenEvent.Skip, OnSkip);
            view.dispatcher.AddListener(CreditsScreenEvent.OtherGames, OnOtherGames);

            view.Init();
        }

        private void OnSkip()
        {
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo() {Name = BaseGameElement.MainScreen});
            Destroy(gameObject);
        }

        private void OnOtherGames()
        {
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo() {Name = BaseGameElement.OtherGamesScreen});
            Destroy(gameObject);
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(CreditsScreenEvent.Skip, OnSkip);
            view.dispatcher.RemoveListener(CreditsScreenEvent.OtherGames, OnOtherGames);
        }
    }
}