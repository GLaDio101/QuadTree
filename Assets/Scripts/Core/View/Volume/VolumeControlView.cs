using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Core.View.Volume
{
    public class VolumeControlView : EventView, IVolumeControlView
    {
        public string LabelPrecision = "0.00";

        public Slider VolumeSlider;

        public AudioMixer AudioMixer;

        public Text VolumeLabel;

        public GameObject UnmuteIcon;

        public GameObject MuteIcon;

        private float _tempValue;

        private bool _active;

        public AudioMixer Mixer
        {
            get { return AudioMixer; }
        }

        public float Level
        {
            get {
                if (UnmuteIcon.activeSelf == true)
                {
                    return 1;
                }
                return 0;
            }
            set
            {
                if (value > 0)
                {
                    _active = true;
                    MuteIcon.SetActive(!_active);
                    UnmuteIcon.SetActive(_active);
                }
                else
                {
                    _active = false;
                    MuteIcon.SetActive(!_active);
                    UnmuteIcon.SetActive(_active);
                }
            }
        }
        public void UpdateIcons()
        {
            _active = !_active;
                MuteIcon.SetActive(!_active);
                UnmuteIcon.SetActive(_active);
        }

        public void OnToggleVolume()
        {
            UpdateIcons();

            dispatcher.Dispatch(VolumeControlEvent.Update);
        }

        public float LinearToDecibel(float linear)
        {
            float dB;

            if (Math.Abs(linear) > 0.05f)
                dB = 20.0f * Mathf.Log10(linear);
            else
                dB = -144.0f;

            return dB;
        }
    }
}
