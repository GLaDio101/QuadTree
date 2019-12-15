using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using Service.Localization;

namespace Project.View.Confirm
{
  public class ConfirmPanelMediator : EventMediator
  {
    [Inject]
    public IConfirmPanelView view { get; set; }

    [Inject]
    public ILocalizationService localizationService { get; set; }

    private ConfirmPanelVo vo
    {
      get { return view.vo as ConfirmPanelVo; }
    }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(ConfirmPanelEvent.Confirm, OnConfirm);
      view.dispatcher.AddListener(ConfirmPanelEvent.Cancel, OnCancel);

      view.Title = localizationService.GetText(vo.Title);
      view.Description = localizationService.GetText(vo.Description);
      view.ConfirmButtonLabel = localizationService.GetText(vo.ButtonLabel);
      view.CancelButtonLabel = localizationService.GetText(vo.CancelButtonLabel);
      view.IconName = vo.Icon;
    }

    private void OnConfirm(IEvent payload)
    {
      if (vo.OnConfirm != null)
        vo.OnConfirm();
      Destroy(gameObject);
    }

    private void OnCancel(IEvent payload)
    {
      if (vo.OnCancel != null)
        vo.OnCancel();
      Destroy(gameObject);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(ConfirmPanelEvent.Confirm, OnConfirm);
      view.dispatcher.RemoveListener(ConfirmPanelEvent.Cancel, OnCancel);
    }
  }
}