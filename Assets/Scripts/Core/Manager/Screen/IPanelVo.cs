namespace Core.Manager.Screen
{
    public delegate void PanelCallback();

    public interface IPanelVo
    {
        string Name { get; set; }

        int LayerIndex { get; set; }

        bool RemoveAll { get; set; }

        bool NotCancellable { get; set; }
        
        bool RemoveLayer { get; set; }
    }
}