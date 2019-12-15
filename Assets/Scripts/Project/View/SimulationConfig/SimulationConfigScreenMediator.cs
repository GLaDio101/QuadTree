using Core.Manager.Screen;
using Project.Enums;
using Project.Model.SimulationConfig;
using strange.extensions.mediation.impl;

namespace Project.View.SimulationConfig
{
    public enum SimulationConfigScreenEvent
    {
        Back
    }

    public class SimulationConfigScreenMediator : EventMediator
    {
        [Inject] public SimulationConfigScreenView view { get; set; }

        [Inject] public ISimulationConfigModel simulationConfigModel { get; set; }

        public override void OnRegister()
        {
            view.Config = simulationConfigModel.Config;

            view.dispatcher.AddListener(SimulationConfigScreenEvent.Back, OnBack);
        }

        private void OnBack()
        {
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo()
            {
                Name = GameElement.MenuScreen
            });
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(SimulationConfigScreenEvent.Back, OnBack);
        }
    }
}