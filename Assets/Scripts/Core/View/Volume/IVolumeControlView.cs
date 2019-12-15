using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine.Audio;

namespace Core.View.Volume
{
    public enum VolumeControlEvent
    {
        Update,
        ToggleMute,
        Mute,
        Unmute
    }

    public interface IVolumeControlView
    {
        IEventDispatcher dispatcher { get; }

        float Level { get; set; }

        AudioMixer Mixer { get; }

        float LinearToDecibel(float linear);
    }
}