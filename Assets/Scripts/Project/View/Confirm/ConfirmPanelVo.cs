using Core.Manager.Screen;

namespace Project.View.Confirm
{
    public class ConfirmPanelVo:IPanelVo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ButtonLabel { get; set; }
        public string CancelButtonLabel { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public int LayerIndex { get; set; }
        public bool RemoveAll { get; set; }
        public bool NotCancellable { get; set; }
        public bool RemoveLayer { get; set; }
        public PanelCallback OnConfirm { get; set; }
        public PanelCallback OnCancel { get; set; }
    }
}