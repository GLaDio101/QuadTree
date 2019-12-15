using strange.extensions.dispatcher.eventdispatcher.api;

namespace Core.Manager.Screen
{
    public interface IPanelView
    {
        IEventDispatcher dispatcher { get; }

        IPanelVo vo { get; set; }
    }
}