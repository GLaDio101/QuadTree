using Core.Manager.Screen;
using Project.Enums;
using Project.Enums.Events;
using strange.extensions.mediation.impl;

namespace Project.View.Menu
{
    public enum MenuScreenEvent
    {
        PlayGame,
        Config
    }

    public class MenuScreenMediator : EventMediator
    {
        [Inject] public MenuScreenView view { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(MenuScreenEvent.PlayGame, OnPlayGame);
            view.dispatcher.AddListener(MenuScreenEvent.Config, OnConfig);
        }

        private void OnConfig()
        {
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo()
            {
                Name = GameElement.SimulationConfigScreen
            });
            Destroy(gameObject);
        }

        public void OnPlayGame()
        {
            dispatcher.Dispatch(GamePlayEvent.PlayGame);
            Destroy(gameObject);
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(MenuScreenEvent.PlayGame, OnPlayGame);
            view.dispatcher.RemoveListener(MenuScreenEvent.Config, OnConfig);
        }
    }
}