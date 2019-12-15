using UnityEngine.Audio;

namespace Project.Model.Game
{
    public class GameModel:IGameModel
    {
        public void Reset()
        {
        }

        public AudioMixer Mixer { get; set; }

        public AudioMixerGroup EffectsMixerGroup { get; set; }
    }
}