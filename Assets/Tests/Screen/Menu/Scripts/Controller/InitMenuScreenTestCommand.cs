#if UNITY_EDITOR
using Core.Manager.Screen;
using strange.extensions.command.impl;

namespace Assets.Tests.Screen.Menu.Scripts.Controller
{
    public class InitMenuScreenTestCommand : EventCommand
    {
        public override void Execute()
        {
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo{Name = "LoadingScreen"});

            //dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo{Name = GameElement.InitMenuScreen} );
        }
    }
}
#endif