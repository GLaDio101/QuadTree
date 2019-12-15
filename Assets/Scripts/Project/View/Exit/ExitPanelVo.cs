using Core.Manager.Screen;

namespace Project.View.Exit
{
    public class ExitPanelVo : IPanelVo
    {
        private int _layerIndex = 1;

        public string Name
        {
            get
            {
                return "ExitPanel" ;
            }
        }

        public int LayerIndex
        {
            get { return _layerIndex; }
            set { _layerIndex = value; }
        }

        public bool RemoveAll { get; set; }

        string IPanelVo.Name { get; set; }
        public bool NotCancellable { get { return true; } set {  } }
        public bool RemoveLayer { get; set; }
    }
}
