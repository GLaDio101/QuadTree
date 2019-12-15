using strange.extensions.mediation.impl;
using TMPro;

namespace Core.View
{
    public class LanguageSelectorView : EventView
    {
        public TextMeshProUGUI LanguageLabel;

        public string Language
        {
            set
            {
                if(LanguageLabel != null)
                    LanguageLabel.text = value;
            }
        }

        public void OnLanguageClick()
        {
            dispatcher.Dispatch(LanguageSelectorEvent.Update);
        }
    }
}
