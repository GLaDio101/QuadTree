using System.Collections.Generic;
using Core.Model;
using strange.extensions.mediation.impl;
using Service.Localization;
using UnityEngine;

namespace Core.View
{
    public enum LanguageSelectorEvent
    {
        Update
    }

    public class LanguageSelectorMediator : EventMediator
    {
        [Inject] public LanguageSelectorView view { get; set; }

        [Inject] public IBasePlayerModel playerModel { get; set; }

        [Inject] public ILocalizationService localizationService { get; set; }

        private List<string> availableLanguages;

        private int currentLanguageIndex;

        public override void OnRegister()
        {
            view.dispatcher.AddListener(LanguageSelectorEvent.Update, OnLanguage);
            availableLanguages = localizationService.GetLanguageCodeList();
            if (availableLanguages == null || availableLanguages.Count == 0)
            {
                Debug.LogWarning("Available Languages is null or empty!!!");
                return;
            }

            currentLanguageIndex = 0;
            foreach (var language in availableLanguages)
            {
                if (language == playerModel.Settings.Language)
                {
                    break;
                }

                currentLanguageIndex++;
            }

            if (currentLanguageIndex >= availableLanguages.Count)
                currentLanguageIndex = 0;

            view.Language = availableLanguages[currentLanguageIndex].ToUpper();
        }

        private void OnLanguage()
        {
            if (availableLanguages == null || availableLanguages.Count == 0)
            {
                Debug.LogWarning("Available Languages is null or empty!!!");
                return;
            }
            currentLanguageIndex++;
            if (currentLanguageIndex > availableLanguages.Count - 1)
                currentLanguageIndex = 0;

            view.Language = availableLanguages[currentLanguageIndex].ToUpper();
            playerModel.Settings.Language = availableLanguages[currentLanguageIndex];
            localizationService.SetLanguageByCode(availableLanguages[currentLanguageIndex]);
        }

        public override void OnRemove()
        {
            availableLanguages = null;

            view.dispatcher.RemoveListener(LanguageSelectorEvent.Update, OnLanguage);
        }
    }
}