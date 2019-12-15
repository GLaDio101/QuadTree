using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.View
{
    public class CreditsScreenView : CoreView,IPointerDownHandler,IPointerUpHandler
    {
        public ScrollRect ScrollRect;

        [Range(0.001f,0.1f)]
        public float Speed = .01f;

        private bool _moving;

        private Vector2 _tempPosition;

        public void Init()
        {
            _tempPosition = Vector2.one;
            _moving = true;
        }

        public void OnSkipClick()
        {
            //StopCoroutine(End());
            DispatchDelayed(CreditsScreenEvent.Skip);
        }

        public void OnOtherGamesClick()
        {
            //StopCoroutine(End());
            DispatchDelayed(CreditsScreenEvent.OtherGames);
        }

        [UsedImplicitly]
        private void Update()
        {
            if (_moving)
            {
                _tempPosition.y -= Speed*Time.deltaTime;
                ScrollRect.normalizedPosition = _tempPosition;
                if (_tempPosition.y <= 0)
                {
                    _moving = false;
                    //StartCoroutine(End());
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                DispatchDelayed(CreditsScreenEvent.Skip);
        }

        //private IEnumerator End()
        //{
        //    yield return new WaitForSecondsRealtime(2f);

        //    dispatcher.Dispatch(CreditsScreenEvent.Skip);
        //}

        public void OnPointerDown(PointerEventData eventData)
        {
            Speed *= 3;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Speed /= 3;
        }
    }
}
