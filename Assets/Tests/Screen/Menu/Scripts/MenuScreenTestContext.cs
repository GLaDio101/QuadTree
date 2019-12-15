#if UNITY_EDITOR
using Assets.Tests.Screen.Alert.Scripts;
using Assets.Tests.Screen.Menu.Scripts.Controller;
using Core.Testing;
using Project.View.Menu;
using strange.extensions.context.api;
using UnityEngine;

namespace Tests.Screen.Menu.Scripts
{
    public class MenuScreenTestContext : BaseTestContext
    {
        public MenuScreenTestContext(MonoBehaviour view) : base(view)
        {
        }

        public MenuScreenTestContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            mediationBinder.Bind<MenuScreenView>().To<MenuScreenMediator>();


            commandBinder.Bind(ContextEvent.START).InSequence().To<AddKeyboardListener>().To<InitMenuScreenTestCommand>();
            commandBinder.Bind("1").To<InitMenuScreenTestCommand>();
        }
    }
}
#endif