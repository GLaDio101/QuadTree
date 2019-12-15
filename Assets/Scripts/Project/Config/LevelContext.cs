using Core.Localization;
using Core.Manager.Audio;
using Core.Manager.Bundle;
using Core.Manager.Scene;
using Core.Manager.Screen;
using Core.Model;
using Core.View;
using Project.Controller.Base;
using Project.Controller.Bootstrap;
using Project.Manager.SimulationManager;
using Project.Model.Bundle;
using Project.Model.Cameras;
using Project.Model.Game;
using Project.Model.Player;
using Project.View.Confirm;
using Project.View.Exit;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using Service.Config;
using Service.Config.Imp;
using Service.Keyboard;
using Service.Keyboard.Imp;
using Service.Localization;
using Service.NetConnection;
using UnityEngine;

//%IMPORTPOINT%

namespace Project.Config
{
    public class LevelContext : MVCSContext
    {
        public LevelContext(MonoBehaviour view) : base(view)
        {
        }

        public LevelContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();


            // models

            // services

//%INJECTIONPOINT%

            //views
            mediationBinder.Bind<SimulationManagerView>().To<SimulationManagerMediator>();

            // room editor views


//%MEDIATIONPOINT%

            commandBinder.Bind(ContextEvent.START).InSequence().To<LevelLoadedCommand>();

            //%COMMANDPOINT%
        }
    }
}