#if UNITY_EDITOR
using Assets.Tests.Screen.Alert.Scripts;
using Assets.Tests.Screen.SimulationConfig.Scripts.Controller;
using Core.Testing;
using Project.View.SimulationConfig;
using strange.extensions.context.api;
using UnityEngine;

namespace Tests.Screen.SimulationConfig.Scripts
{
    public class SimulationConfigScreenTestContext : BaseTestContext
    {
        public SimulationConfigScreenTestContext(MonoBehaviour view) : base(view)
        {
        }

        public SimulationConfigScreenTestContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            mediationBinder.Bind<SimulationConfigScreenView>().To<SimulationConfigScreenMediator>();


            commandBinder.Bind(ContextEvent.START).InSequence().To<AddKeyboardListener>()
                .To<InitSimulationConfigScreenTestCommand>();
            commandBinder.Bind("1").To<InitSimulationConfigScreenTestCommand>();
        }
    }
}
#endif