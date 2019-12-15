using UnityEngine.Audio;

namespace Core.Model
{
    public interface IBaseGameModel
    {
        void Reset();

        AudioMixer Mixer { get; set; }
        
        AudioMixerGroup EffectsMixerGroup { get; set; }
    }
}