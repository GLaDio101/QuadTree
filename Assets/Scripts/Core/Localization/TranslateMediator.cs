using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using Service.Localization;

namespace Core.Localization
{
    public enum TranslateEvent
    {
        KeyChanged,
        Start
    }

    public class TranslateMediator : EventMediator
    {
        [Inject]
        public Translate view { get; set; }

        [Inject]
        public ILocalizationService localizationService { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(TranslateEvent.KeyChanged, OnKeyChanged);
            view.dispatcher.AddListener(TranslateEvent.Start, OnStart);
            dispatcher.AddListener(LocalizationEvent.LanguageChanged, OnLanguageChanged);
            view.Init();
        }

        private void OnStart()
        {
            view.SetText(localizationService.GetText(view.Key));
        }

        private void OnKeyChanged()
        {
            //Debug.Log(view.Key);
            //Debug.Log(localizationService.GetText(view.Key));
            view.SetText(localizationService.GetText(view.Key));
        }

        private void OnLanguageChanged(IEvent payload)
        {
//            view.Textfix(localizationService.CurrentLanguageCode);

            view.SetText(localizationService.GetText(view.Key));
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(TranslateEvent.KeyChanged, OnKeyChanged);
            dispatcher.RemoveListener(LocalizationEvent.LanguageChanged, OnLanguageChanged);
            view.dispatcher.RemoveListener(TranslateEvent.Start, OnStart);
        }
    }
}