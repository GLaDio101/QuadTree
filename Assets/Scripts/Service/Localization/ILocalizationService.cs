using System.Collections.Generic;

namespace Service.Localization
{
    public interface ILocalizationService
    {
        string GetText(string key, object[] args = null);

        string CurrentLanguageCode { get; }

        void SetLanguageByCode(string code);

        string GetDefaultLanguageCode();

        void NextLanguage();

        List<string> GetLanguageCodeList();
    }
}