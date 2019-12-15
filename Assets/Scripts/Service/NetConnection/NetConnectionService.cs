using System;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.Networking;

namespace Service.NetConnection
{
    public class NetConnectionService : INetConnectionService
    {
        private const float IntervalTestStart = 1;
        private const float IntervalTestReachable = 5;
        private const float IntervalTestNotReachable = 5;

        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject contextView { get; set; }

        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        public bool Available
        {
            get { return Status == NetConnectionStatus.Reachable; }
        }

        public NetConnectionStatus Status
        {
            get { return _status; }
            private set
            {
                if (_status == value) return;

                _status = value;
                switch (_status)
                {
                    case NetConnectionStatus.NotReachable:
                        _currentDelay = IntervalTestNotReachable;
                        break;

                    case NetConnectionStatus.Reachable:
                        _currentDelay = IntervalTestReachable;
                        break;
                }

                dispatcher.Dispatch(NetConnectionEvent.StatusChanged);
            }
        }

        public bool Auto
        {
            get { return _auto; }
            set
            {
                _auto = value;
                MonoBehaviour root = contextView.GetComponent<ContextView>();
                if (_auto)
                    root.StartCoroutine(ServiceRoutine());
                else
                    root.StopCoroutine(ServiceRoutine());
            }
        }

        private bool _isInited;

        private bool _auto;

        private NetConnectionStatus _status = NetConnectionStatus.WaitForSignal;

        private float _currentDelay = IntervalTestStart;

        public void Init(bool auto = false)
        {
            if (_isInited) return;
            _isInited = true;
            _auto = auto;

            Status = NetConnectionStatus.WaitForSignal;

            _currentDelay = IntervalTestStart;

            StartRoutine();
        }

        public void Check()
        {
            StartRoutine();
        }

        private void StartRoutine()
        {
            MonoBehaviour root = contextView.GetComponent<ContextView>();
            if (_auto)
                root.StartCoroutine(ServiceRoutine());
            else
            {
                //Status = NetConnectionStatus.WaitForSignal;
                root.StartCoroutine(Test());
            }
        }

        private IEnumerator ServiceRoutine()
        {
            yield return new WaitForSeconds(_currentDelay);
            //Status = NetConnectionStatus.WaitForSignal;
            yield return Test();

            StartRoutine();
        }

        private IEnumerator Test()
        {
            using (var www = new UnityWebRequest("https://google.com"))
            {
                yield return www;

                NetConnectionStatus status = NetConnectionStatus.NotReachable;

                if (www.error == null)
                {
                    if (www.GetResponseHeaders() != null)
                        if (www.GetResponseHeaders().ContainsKey("SERVER") && www.GetResponseHeader("SERVER") == "gws")
                            status = NetConnectionStatus.Reachable;
                    else if (www.downloadHandler != null)
                        if (www.downloadHandler.text.IndexOf("schema.org/WebPage", StringComparison.Ordinal) > -1)
                            status = NetConnectionStatus.Reachable;
                }

                Status = status;
            }
        }
    }
}