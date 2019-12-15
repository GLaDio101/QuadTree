#if UNITY_EDITOR
using Core.Localization;
using Core.Manager.Audio;
using Core.Manager.Bundle;
using Core.Manager.Scene;
using Core.Manager.Screen;
using Core.Model;
using Core.Testing;
using Project.Enums.Events;
using Project.Model.Bundle;
using Project.Model.Cameras;
using Project.Model.Game;
using Project.Model.Player;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using Service;
using Service.Ad;
using Service.Ad.Interfaces;
using Service.Config;
using Service.Config.Imp;
using Service.Keyboard;
using Service.Keyboard.Imp;
using Service.Localization;
using Service.NetConnection;
using Service.Save;
using Service.Save.Imp;
using Service.Tracking;
using UnityEngine;

//%IMPORTPOINT%

namespace Assets.Tests.Screen.Alert.Scripts
{
  public class BaseTestContext : MVCSContext
  {
    public BaseTestContext(MonoBehaviour view) : base(view)
    {
    }

    public BaseTestContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      base.mapBindings();

      CrossContextEvent<GamePlayEvent>();
      CrossContextEvent<ScreenEvent>();

      // base models
      injectionBinder.Bind<IPlayerModel>().Bind<IBasePlayerModel>().To<PlayerModel>().ToSingleton()
        .CrossContext();
      injectionBinder.Bind<IScreenModel>().To<ScreenModel>().ToSingleton().CrossContext();
      injectionBinder.Bind<ISceneModel>().To<DummySceneModel>().ToSingleton().CrossContext();
      injectionBinder.Bind<IGameModel>().Bind<IBaseGameModel>().To<GameModel>().ToSingleton().CrossContext();
      injectionBinder.Bind<IBundleModel>().To<BundleModel>().ToSingleton().CrossContext();
      injectionBinder.Bind<IBundleInfoModel>().Bind<IProjectBundleInfoModel>().To<ProjectBundleInfoModel>()
        .ToSingleton().CrossContext();
      injectionBinder.Bind<ICameraModel>().To<CameraModel>().ToSingleton().CrossContext();
      // base services
      injectionBinder.Bind<IAdService>().To<DummyAdService>().ToSingleton().CrossContext();
      injectionBinder.Bind<ISaveService>().To<LocalSaveService>().ToSingleton().CrossContext();
      injectionBinder.Bind<INetConnectionService>().To<NetConnectionService>().ToSingleton().CrossContext();
      injectionBinder.Bind<ILocalizationService>().To<I2LocalizationService>().ToSingleton().CrossContext();

      // project models

      // project services
      injectionBinder.Bind<IKeyboardService>().To<DummyKeyboardService>().ToSingleton().CrossContext();
      injectionBinder.Bind<IConfigService>().To<DummyConfigService>().ToSingleton().CrossContext();

//%INJECTIONPOINT%

      // views
      mediationBinder.Bind<ScreenManager>().To<ScreenManagerMediator>();
      mediationBinder.Bind<AudioManager>().To<AudioManagerMediator>();
      mediationBinder.Bind<Translate>().To<TranslateMediator>();


      // test only
      mediationBinder.Bind<KeyboardListenerView>().To<KeyboardListenerMediator>();
      //%MEDIATIONPOINT%

      //%COMMANDPOINT%
    }
  }
}
#endif