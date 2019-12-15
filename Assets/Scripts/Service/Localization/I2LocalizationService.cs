using System.Collections.Generic;
using ArabicSupport;
using I2.Loc;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Service.Localization
{
    public class I2LocalizationService : ILocalizationService
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
            LocalizationManager.OnLocalizeEvent += OnLocalizeEvent;
        }

        private void OnLocalizeEvent()
        {
//            LocalizationManager.CurrentLanguageCode = thislanguagemanager.CurrentlyLoadedCulture.languageCode;
            dispatcher.Dispatch(LocalizationEvent.LanguageChanged);
        }


        public string GetText(string key, object[] args = null)
        {
            string textValue = LocalizationManager.GetTranslation(key);
            if (string.IsNullOrEmpty(textValue))
                return key;

            if (args != null)
                textValue = string.Format(textValue, args);

            if (CurrentLanguageCode == "ar")
            {
                textValue = ArabicFixer.Fix(textValue);
            }

            return textValue;
        }


        public string CurrentLanguageCode
        {
            get { return LocalizationManager.CurrentLanguageCode; }
        }

        public string GetDefaultLanguageCode()
        {
            return "en";
        }

        public void SetLanguageByCode(string code)
        {
            PlayerPrefs.SetString("language", code);

            LocalizationManager.CurrentLanguageCode = code;
        }

        public void NextLanguage()
        {
            string currentLanguage = LocalizationManager.CurrentLanguageCode;

            int index = 0;
            foreach (var languageCode in LocalizationManager.GetAllLanguagesCode())
            {
                if (languageCode == currentLanguage)
                {
                    break;
                }

                index++;
            }

            index++;
            if (index == LocalizationManager.GetAllLanguagesCode().Count)
                index = 0;

            LocalizationManager.CurrentLanguageCode = LocalizationManager.GetAllLanguagesCode()[index];
        }

        public List<string> GetLanguageCodeList()
        {
            return LocalizationManager.GetAllLanguagesCode();
        }
    }
}