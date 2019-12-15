#if UNITY_EDITOR
using Core.Manager.Screen;
using strange.extensions.command.impl;

namespace Assets.Tests.Screen.SimulationConfig.Scripts.Controller
{
    public class InitSimulationConfigScreenTestCommand : EventCommand
    {
        public override void Execute()
        {
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo{Name = "LoadingScreen"});

            //dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo{Name = GameElement.InitSimulationConfigScreen} );
        }
    }
}
#endif