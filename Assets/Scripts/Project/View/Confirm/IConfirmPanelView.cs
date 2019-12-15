using Core.Manager.Screen;

namespace Project.View.Confirm
{
    public enum ConfirmPanelEvent
    {
        Cancel,
        Confirm
    }

    public interface IConfirmPanelView:IPanelView
    {
        string Title { set; }

        string Description { set; }

        string ConfirmButtonLabel { set; }

        string CancelButtonLabel { set; }

        string IconName { set; }
    }
}