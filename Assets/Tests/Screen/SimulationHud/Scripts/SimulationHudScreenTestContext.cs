#if UNITY_EDITOR
using Core.Testing;
using Assets.Tests.Screen.Alert.Scripts;
using Assets.Tests.Screen.SimulationHud.Scripts.Controller;
using Project.View.SimulationHud;
using strange.extensions.context.api;
using UnityEngine;

namespace Assets.Tests.Screen.SimulationHud.Scripts
{
    public class SimulationHudScreenTestContext : BaseTestContext
    {
        public SimulationHudScreenTestContext(MonoBehaviour view) : base(view)
        {
        }

        public SimulationHudScreenTestContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            mediationBinder.Bind<SimulationHudScreenView>().To<SimulationHudScreenMediator>();


            commandBinder.Bind(ContextEvent.START).InSequence().To<AddKeyboardListener>().To<InitSimulationHudScreenTestCommand>();
            commandBinder.Bind("1").To<InitSimulationHudScreenTestCommand>();
        }
    }
}
#endif