#if UNITY_EDITOR
using Core.Manager.Screen;
using strange.extensions.command.impl;

namespace Assets.Tests.Screen.SimulationHud.Scripts.Controller
{
    public class InitSimulationHudScreenTestCommand : EventCommand
    {
        public override void Execute()
        {
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo{Name = "LoadingScreen"});

            //dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo{Name = GameElement.InitSimulationHudScreen} );
        }
    }
}
#endif