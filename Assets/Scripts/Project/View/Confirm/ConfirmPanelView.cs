using Core.Manager.Screen;
using Core.View;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.View.Confirm
{
    public class ConfirmPanelView : CoreView, IConfirmPanelView
    {
        public TextMeshProUGUI TitleLabel;

        public TextMeshProUGUI DescriptionLabel;

        public TextMeshProUGUI CancelButtonText;

        public TextMeshProUGUI ConfirmButtonText;

        public Image Icon;

        public void OnCancelClick()
        {
            DispatchDelayed(ConfirmPanelEvent.Cancel);
        }

        public void OnConfirmClick()
        {
            DispatchDelayed(ConfirmPanelEvent.Confirm);
        }

        [UsedImplicitly]
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !vo.NotCancellable)
                DispatchDelayed(ConfirmPanelEvent.Cancel);
        }

        public string Title
        {
            set
            {
                if (TitleLabel != null)
                    TitleLabel.text = value;

            }
        }

        public string Description
        {
            set
            {
                if (DescriptionLabel != null)
                    DescriptionLabel.text = value;
            }
        }

        public string ConfirmButtonLabel
        {
            set
            {
                if (ConfirmButtonText != null)
                    ConfirmButtonText.text = value;
            }
        }

        public string CancelButtonLabel
        {
            set
            {
                if (CancelButtonText != null)
                    CancelButtonText.text = value;
            }
        }

        public string IconName
        {
            set
            {
                if (Icon != null)
                {
                    Icon.sprite = Resources.Load<Sprite>("GUI/Sprites/" + value);
                    Icon.SetNativeSize();
                }
            }
        }

        public IPanelVo vo { get; set; }
    }
}
