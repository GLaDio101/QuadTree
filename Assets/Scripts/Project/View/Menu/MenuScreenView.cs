using strange.extensions.mediation.impl;

namespace Project.View.Menu
{
    public class MenuScreenView : EventView
    {
        public void OnPlayGameClick()
        {
            dispatcher.Dispatch(MenuScreenEvent.PlayGame);
        }

        public void OnConfigClick()
        {
            dispatcher.Dispatch(MenuScreenEvent.Config);
        }
    }
}