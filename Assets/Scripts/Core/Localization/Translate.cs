using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;

namespace Core.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Translate : EventView
    {
        private TextMeshProUGUI _text;

        public string Key;

        public void Init()
        {
            base.Start();
            _text = gameObject.GetComponent<TextMeshProUGUI>();
            if (_text != null)
            {
                if (Key == string.Empty)
                    Key = gameObject.name;

                if (Key == string.Empty)
                    Key = _text.text;

                dispatcher.Dispatch(TranslateEvent.Start);
            }
        }

        public void SetText(string value)
        {
            if (_text == null)
                return;

            _text.text = value;
        }

        public void SetKey(string value)
        {
            Key = value;
            if (dispatcher != null)
                dispatcher.Dispatch(TranslateEvent.KeyChanged);
        }


    }
}