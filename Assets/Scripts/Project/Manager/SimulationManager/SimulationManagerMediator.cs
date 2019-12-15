using Project.Model.SimulationConfig;
using strange.extensions.mediation.impl;

namespace Project.Manager.SimulationManager
{
    public class SimulationManagerMediator : EventMediator
    {
        [Inject] public SimulationManagerView view { get; set; }

        [Inject] public ISimulationConfigModel simulationConfigModel { get; set; }

        public override void OnRegister()
        {
            view.ConfigVo = simulationConfigModel.Config;
        }


        public override void OnRemove()
        {
        }
    }
}