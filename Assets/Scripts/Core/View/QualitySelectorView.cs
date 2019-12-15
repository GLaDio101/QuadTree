using Core.Localization;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Core.View
{
    public class QualitySelectorView : EventView
    {
        public Translate LabelTransator;

        public string QualityLevelLanguageKeyPrefix = "QualityLevel_";

        public short LevelCount = 3;

        public Image ImageField;

        public Sprite[] Sprites;

        public short Level
        {
            set
            {
                LabelTransator.SetKey(QualityLevelLanguageKeyPrefix );
                ImageField.sprite = Sprites[value];
            }
        }

        public void OnQualityClick(int value)
        {
            dispatcher.Dispatch(QualitySelectorEvent.Update, value);
        }
    }
}
