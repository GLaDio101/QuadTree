using System;
using System.Collections;
using Core.Model;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Core.Manager.Audio
{
    public class AudioManagerMediator : EventMediator
    {
        [Inject]
        public AudioManager view { get; set; }

        [Inject]
        public IBasePlayerModel playerModel { get; set; }

        [Inject]
        public IBaseGameModel gameModel { get; set; }

        private float _effectsTemp = 1;

        private float _musicTemp = 1;

        public override void OnRegister()
        {
            dispatcher.AddListener(AudioEvent.Play, OnPlayAudio);
            dispatcher.AddListener(AudioEvent.ToggleEffects, OnToggleEffects);
            dispatcher.AddListener(AudioEvent.ToggleMusic, OnToggleMusic);
            dispatcher.AddListener(AudioEvent.Update, UpdateVolume);
            dispatcher.AddListener(AudioEvent.StartThemeSong, OnStartThemeSong);
            dispatcher.AddListener(AudioEvent.StopThemeSong, OnStopThemeSong);

            gameModel.EffectsMixerGroup = view.mixer.FindMatchingGroups("SoundEffects")[0];
        }

        private void OnStartThemeSong(IEvent payload)
        {
            if (!view.ThemeSong.isPlaying)
            {
                view.ThemeSong.volume = 0;
                view.ThemeSong.Play();
                StartCoroutine(FadeIn(view.ThemeSong, 1f));
            }
        }

        private void OnStopThemeSong(IEvent payload)
        {
            if (view.ThemeSong.isPlaying)
                StartCoroutine(FadeOut(view.ThemeSong, 1f));
        }

        private IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
        {
            while (audioSource.volume < 1)
            {
                audioSource.volume += 1 * Time.deltaTime / fadeTime;

                yield return null;
            }
        }

        private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= 1 * Time.deltaTime / fadeTime;

                yield return null;
            }

            audioSource.Pause();
        }

        private void UpdateVolume()
        {
            if (playerModel.Settings.AdvancedVolume)
            {
                SetMusic();
                SetSfx();
            }
            else
            {
                AudioListener.volume = playerModel.Settings.Volume;
            }
        }

        private void OnToggleMusic()
        {
            if (playerModel.Settings.Music > 0)
            {
                _musicTemp = playerModel.Settings.Music;
                playerModel.Settings.Music = 0;
            }
            else
            {
                playerModel.Settings.Music = _musicTemp;
            }

            SetMusic();
        }

        public void SetMusic()
        {
            view.mixer.SetFloat("MusicVol", LinearToDecibel(playerModel.Settings.Music));
        }

        private float LinearToDecibel(float linear)
        {
            float dB;

            if (Math.Abs(linear) > 0.05f)
                dB = 20.0f * Mathf.Log10(linear);
            else
                dB = -144.0f;

            return dB;
        }

        public void SetSfx()
        {
            view.mixer.SetFloat("SfxVol", LinearToDecibel(playerModel.Settings.Effects * 80 - 80));
        }

        private void OnToggleEffects()
        {
            if (playerModel.Settings.Effects > 0)
            {
                _effectsTemp = playerModel.Settings.Effects;
                playerModel.Settings.Effects = 0;
            }
            else
            {
                playerModel.Settings.Effects = _effectsTemp;
            }

            SetSfx();
        }

        private void OnPlayAudio(IEvent payload)
        {
            var index = (int)payload.data;

            if (index < view.sourceList.Length)
                view.sourceList[index].Play();
        }

        public override void OnRemove()
        {
            dispatcher.RemoveListener(AudioEvent.Play, OnPlayAudio);
            dispatcher.RemoveListener(AudioEvent.ToggleEffects, OnToggleEffects);
            dispatcher.RemoveListener(AudioEvent.ToggleMusic, OnToggleMusic);
            dispatcher.RemoveListener(AudioEvent.Update, UpdateVolume);
            dispatcher.RemoveListener(AudioEvent.StartThemeSong, OnStartThemeSong);
            dispatcher.RemoveListener(AudioEvent.StopThemeSong, OnStopThemeSong);
        }
    }
}