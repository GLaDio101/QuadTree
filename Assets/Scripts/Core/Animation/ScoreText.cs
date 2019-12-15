using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Core.Animation
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        //private AudioSource _audioSource;

        private float _value;

        private float _targetValue;

        private bool _animating;

        private string _tempText;

        [Range(0,1)]
        public float Duration = 0.3f;

        public bool AutoPlay { get; set; }

        public string Format { get; set; }

        public string PreText { get; set; }

        public string PostText { get; set; }

        public Color EndColor { get; set; }

        public bool ChangeColor { get; set; }

        public float Value
        {
            get { return _value; }
            set
            {
                _targetValue = value;
                if (AutoPlay)
                    Play();
            }
        }

        public void Play()
        {
            if (Math.Abs(_value - _targetValue) > 0.01f)
            {
                _animating = true;
            }
            else
            {
                Finish();
            }
        }

        [UsedImplicitly]
        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            Format = "0";
            AutoPlay = false;
        }

        [UsedImplicitly]
        private void Update()
        {
            if (!_animating)
                return;

            _value = Mathf.Lerp(_value, _targetValue, Duration * Time.timeScale);

            if (Math.Abs(_value - _targetValue) < 0.01f)
            {
                _value = _targetValue;
                Finish();
            }

            _tempText = PreText + _value.ToString(Format) + PostText;

            if (_text.text != _tempText)
            {
                _text.text = _tempText;
            }
        }

        private void Finish()
        {
            if(ChangeColor)
                _text.color = EndColor;
            _animating = false;
            SendMessageUpwards("NextAnimation", SendMessageOptions.DontRequireReceiver);
        }
    }
}