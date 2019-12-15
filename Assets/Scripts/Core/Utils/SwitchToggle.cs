using UnityEngine;
using UnityEngine.UI;

namespace Core.Utils
{
    [AddComponentMenu("UI/SwitchToggle")]
    public class SwitchToggle : Toggle
    {
        public Image ActiveImage;

        public Image DisableImage;

        protected override void Start()
        {
            onValueChanged.AddListener(ValueChanged);
            ValueChanged(isOn);
        }

        public void ValueChanged(bool value)
        {
            if (graphic == null)
                return;


            if (value)
            {
                ActiveImage.enabled = true;
                DisableImage.enabled = false;
            }
            else
            {
                DisableImage.enabled = true;
                ActiveImage.enabled = false;
            }
        }
    }
}