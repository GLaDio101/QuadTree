using UnityEngine;

namespace Service.Ad.Player
{
    public delegate void SkipHandler();

    public interface IAdVideoPlayer
    {
        void Load(string videoId);

        SkipHandler OnSkip { get; set; }

        SkipHandler OnCompleted { get; set; }

        SkipHandler OnClick { get; set; }

        Camera Camera { set; }

        void Play();

        void ShowSkip(int duration);
    }
}