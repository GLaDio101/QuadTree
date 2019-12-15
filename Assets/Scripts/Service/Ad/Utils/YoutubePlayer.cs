/*using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using RenderHeads.Media.AVProVideo;
using Service.Ad.Player;
using UnityEngine;
using UnityEngine.UI;
using YoutubeLight;

namespace Service.Ad.Utils
{
    public class YoutubePlayer : MonoBehaviour, IAdVideoPlayer
    {
        private string _videoId;

        private string _videoUrl;

        private bool _videoAreReadyToPlay;

        public MediaPlayer MediaPlayer;

        RequestResolver _resolver;

        public GameObject SkipButton;

        public GameObject VideoLayer;

        //bool _checkIfVideoArePrepared;

        //private bool _isFocused;

        private bool _completed;

        private bool _loading;

        private bool _closed;

        public bool Completed
        {
            get { return _completed; }
        }

        public Camera Camera
        {
            //set { MediaPlayer.targetCamera = value; }
            set { }
        }

        public SkipHandler OnSkip { get; set; }

        public SkipHandler OnCompleted { get; set; }

        public SkipHandler OnClick { get; set; }

        public void Init()
        {
            _resolver = gameObject.AddComponent<RequestResolver>();
            VideoLayer.SetActive(false);
            SkipButton.SetActive(false);
        }

        public void Load(string videoId)
        {
            if (_loading)
                return;
            //Debug.LogWarning("load");
            Close();
            _closed = false;
            if (this._videoId != videoId)
            {
                _loading = true;
                this._videoId = videoId;
                StartCoroutine(_resolver.GetDownloadUrls(FinishLoadingUrls, this._videoId, false));
            }
            else
            {
                _videoAreReadyToPlay = true;
            }
        }

        private void FinishLoadingUrls()
        {
            //Debug.Log("asdasdas");
            List<VideoInfo> videoInfos = _resolver.videoInfos;
            foreach (VideoInfo info in videoInfos)
            {
                if (info.VideoType == VideoType.Mp4 && info.Resolution == 360)
                {
                    if (info.RequiresDecryption)
                    {
                        //The string is the video url
                        _videoAreReadyToPlay = false;
                        _loading = false;
                        StartCoroutine(_resolver.DecryptDownloadUrl(DecryptionFinished, info));
                        break;
                    }

                    _videoUrl = info.DownloadUrl;
                    //Debug.Log("videoAreReadyToPlay");
                    _loading = false;
                    _videoAreReadyToPlay = true;
                    break;
                }
            }
        }

        public void DecryptionFinished(string url)
        {
            //Debug.Log("videoAreReadyToPlay");
            _videoUrl = url;
            _loading = false;
            _videoAreReadyToPlay = true;
        }

        [UsedImplicitly]
        private void FixedUpdate()
        {
            //used this to play in main thread.
            if (_videoAreReadyToPlay)
            {
                _videoAreReadyToPlay = false;
                //Debug.Log("Play!!" + _videoUrl);
                //MediaPlayer.url = _videoUrl;
                //_checkIfVideoArePrepared = true;
                //MediaPlayer.Prepare();
                MediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, _videoUrl, false);
            }
        }

        //private IEnumerator PreparingAudio()
        //{
        //    //Wait until video is prepared
        //    WaitForSeconds waitTime = new WaitForSeconds(1);
        //    while (!MediaPlayer.isPrepared)
        //    {
        //        //Debug.Log("Preparing Video");
        //        //Prepare/Wait for 5 sceonds only
        //        yield return waitTime;
        //        //Break out of the while loop after 5 seconds wait
        //        //break;
        //    }

        //    //Debug.Log("Done Preparing Video");

        //    //Play Video
        //    MediaPlayer.Play();

        //    //Play Sound
        //    MediaPlayer.Play();

        //    VideoLayer.GetComponent<Button>().onClick.AddListener(Click);
        //    VideoLayer.SetActive(true);

        //    //Debug.Log("Playing Video" + UnityVideoPlayer.isPlaying);
        //    while (MediaPlayer.isPlaying || !_isFocused)
        //    {
        //        yield return null;
        //    }

        //    OnVideoFinished();
        //}

        public void Play()
        {
            //Debug.Log("play");
            _completed = false;
            //MediaPlayer.targetCamera = Camera.main;

            //if (_checkIfVideoArePrepared)
            //    StartCoroutine(PreparingAudio());
            VideoLayer.GetComponent<DisplayUGUI>().CurrentMediaPlayer = MediaPlayer;
            MediaPlayer.Play();
            //MediaPlayer.Control.
            VideoLayer.GetComponent<Button>().onClick.AddListener(Click);
            VideoLayer.SetActive(true);
            MediaPlayer.Events.AddListener(OnVideoEvents);
        }

        private void OnVideoEvents(MediaPlayer player, MediaPlayerEvent.EventType eventType, ErrorCode errorCode)
        {
            if (eventType == MediaPlayerEvent.EventType.FinishedPlaying)
                OnVideoFinished();
        }

        public void ShowSkip(int duration)
        {
            if (duration == 0)
                ActivateSkip();
            else
                StartCoroutine(ShowSkipAfter(duration));
        }

        private IEnumerator ShowSkipAfter(int duration)
        {
            yield return new WaitForSecondsRealtime(duration);

            if (_completed)
                yield break;

            ActivateSkip();
        }

        public void Skip()
        {
            OnSkip.Invoke();
            Close();
        }

        public void Click()
        {
            ActivateSkip();
            OnClick.Invoke();
        }

        private void ActivateSkip()
        {
            SkipButton.GetComponent<Button>().onClick.AddListener(Skip);
            SkipButton.SetActive(true);
        }

        //[UsedImplicitly]
        //private void OnApplicationFocus(bool hasFocus)
        //{
        //    _isFocused = hasFocus;
        //}

        public void OnVideoFinished()
        {
            OnCompleted.Invoke();
            Close();
        }

        private void Close()
        {
            if (_closed)
                return;
            //Debug.Log("close");
            MediaPlayer.Stop();
            //MediaPlayer.targetCamera = null;
            SkipButton.GetComponent<Button>().onClick.RemoveListener(Skip);
            VideoLayer.GetComponent<Button>().onClick.RemoveListener(Click);
            SkipButton.SetActive(false);
            VideoLayer.SetActive(false);
            _completed = true;
            _videoAreReadyToPlay = false;
            _closed = true;
        }

        public static IAdVideoPlayer GetInstance(Transform parent)
        {
            GameObject playerObject = new GameObject("YoutubePlayer");
            playerObject.transform.SetParent(parent);
            playerObject.AddComponent<RectTransform>();
            YoutubePlayer youtubePlayer = playerObject.AddComponent<YoutubePlayer>();
            MediaPlayer mediaPLayer = playerObject.AddComponent<MediaPlayer>();
            AudioSource audioSource = playerObject.AddComponent<AudioSource>();
            GameObject canvas = GameObject.Find("AdCanvas");
            youtubePlayer.SkipButton = canvas.transform.GetChild(1).gameObject;
            youtubePlayer.VideoLayer = canvas.transform.GetChild(0).gameObject;
            youtubePlayer.MediaPlayer = mediaPLayer;
            youtubePlayer.Init();
            //mediaPLayer.
            //mediaPLayer.waitForFirstFrame = true;
            //mediaPLayer.renderMode = VideoRenderMode.CameraNearPlane;
            //mediaPLayer.aspectRatio = VideoAspectRatio.FitOutside;
            //mediaPLayer.targetCamera = Camera.main;
            //mediaPLayer.controlledAudioTrackCount = 1;
            //mediaPLayer.SetTargetAudioSource(0, audioSource);
            audioSource.volume = .5f;
            return youtubePlayer;
        }
    }
}
*/