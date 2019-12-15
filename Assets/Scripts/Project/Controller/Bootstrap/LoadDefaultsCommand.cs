using Core.Manager.Screen;
using Project.Enums;
using Project.Model.Player;
using strange.extensions.command.impl;
using Service.Localization;
using Service.NetConnection;

namespace Project.Controller.Bootstrap
{
  public class LoadDefaultsCommand : EventCommand
  {
    [Inject]
    public INetConnectionService netConnectionService { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }

    [Inject]
    public ILocalizationService localizationService { get; set; }

    public override void Execute()
    {
#if !UNITY_WEBGL
      netConnectionService.Init();
#endif

      localizationService.SetLanguageByCode(playerModel.Settings.Language);
        
        dispatcher.Dispatch(ScreenEvent.OpenPanel,new PanelVo()
        {
            Name = GameElement.MenuScreen
        });

    }
  }
}