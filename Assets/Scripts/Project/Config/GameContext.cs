using Core.Localization;
using Core.Manager.Audio;
using Core.Manager.Bundle;
using Core.Manager.Scene;
using Core.Manager.Screen;
using Core.Model;
using Core.Testing;
using Core.View;
using Project.Controller.Base;
using Project.Controller.Bootstrap;
using Project.Enums.Events;
using Project.GameEntity.Camera;
using Project.Model.Bundle;
using Project.Model.Cameras;
using Project.Model.Game;
using Project.Model.Player;
using Project.Model.SimulationConfig;
using Project.View.Confirm;
using Project.View.Exit;
using Project.View.Menu;
using Project.View.SimulationConfig;
using Project.View.SimulationHud;
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
    public class GameContext : MVCSContext
    {
        public GameContext(MonoBehaviour view) : base(view)
        {
        }

        public GameContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            CrossContextEvent<GamePlayEvent>();
            CrossContextEvent<ScreenEvent>();

            // models
            injectionBinder.Bind<IPlayerModel>().Bind<IBasePlayerModel>().To<PlayerModel>().ToSingleton()
                .CrossContext();
            injectionBinder.Bind<IScreenModel>().To<ScreenModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGameModel>().Bind<IBaseGameModel>().To<GameModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<ISceneModel>().To<SceneModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IBundleInfoModel>().Bind<IProjectBundleInfoModel>().To<ProjectBundleInfoModel>()
                .ToSingleton().CrossContext();
            injectionBinder.Bind<IBundleModel>().To<BundleModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICameraModel>().To<CameraModel>().ToSingleton().CrossContext();

            // services
            injectionBinder.Bind<IKeyboardService>().To<TouchKeyboardService>().ToSingleton().CrossContext();
            injectionBinder.Bind<IConfigService>().To<DummyConfigService>().ToSingleton().CrossContext();
            injectionBinder.Bind<INetConnectionService>().To<NetConnectionService>().ToSingleton().CrossContext();
            injectionBinder.Bind<ISimulationConfigModel>().To<SimulationConfigModel>().ToSingleton().CrossContext();

            injectionBinder.Bind<ILocalizationService>().To<I2LocalizationService>().ToSingleton().CrossContext();

//%INJECTIONPOINT%

            //views
            mediationBinder.Bind<ScreenManager>().To<ScreenManagerMediator>();
            mediationBinder.Bind<AudioManager>().To<AudioManagerMediator>();
            mediationBinder.Bind<ConfirmPanelView>().ToAbstraction<IConfirmPanelView>().To<ConfirmPanelMediator>();
            mediationBinder.Bind<ExitPanelView>().To<ExitPanelMediator>();
            mediationBinder.Bind<CameraView>().To<CameraMediator>();

            mediationBinder.Bind<Translate>().To<TranslateMediator>();
            mediationBinder.Bind<LanguageSelectorView>().To<LanguageSelectorMediator>();

            mediationBinder.Bind<MenuScreenView>().To<MenuScreenMediator>();
            mediationBinder.Bind<SimulationConfigScreenView>().To<SimulationConfigScreenMediator>();
            mediationBinder.Bind<SimulationHudScreenView>().To<SimulationHudScreenMediator>();


//%MEDIATIONPOINT%

            commandBinder.Bind(ContextEvent.START).InSequence().To<AddServiceProcessorsCommand>()
                .To<LoadBundlesCommand>()
                .To<LoadDefaultsCommand>();

            commandBinder.Bind(GamePlayEvent.PlayGame).To<LoadPlayGameCommand>();

            //%COMMANDPOINT%
        }
    }
}