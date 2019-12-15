using strange.extensions.mediation.impl;

namespace Project.View.SimulationHud
{
    public class SimulationHudScreenView : EventView
    {
        public void OnClickMenu()
        {
            dispatcher.Dispatch(SimulationHudScreenEvent.Menu);
        }
    }
}