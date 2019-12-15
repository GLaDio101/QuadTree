using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Utils
{
    
    public static class InputHelper
    {
        public static bool IsPointerOverUiObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        private static TouchCreator _lastFakeTouch;

        public static List<Touch> GetTouches()
        {
            List<Touch> touches = new List<Touch>();
            touches.AddRange(Input.touches);
            // Uncomment if you want it only to allow mouse swipes in the Unity Editor
            //#if UNITY_EDITOR
            if (_lastFakeTouch == null)
            {
                _lastFakeTouch = new TouchCreator();
            }
            if (Input.GetMouseButtonDown(0))
            {
                _lastFakeTouch.phase = TouchPhase.Began;
                _lastFakeTouch.deltaPosition = new Vector2(0, 0);
                _lastFakeTouch.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                _lastFakeTouch.fingerId = 0;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _lastFakeTouch.phase = TouchPhase.Ended;
                Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                _lastFakeTouch.deltaPosition = newPosition - _lastFakeTouch.position;
                _lastFakeTouch.position = newPosition;
                _lastFakeTouch.fingerId = 0;
            }
            else if (Input.GetMouseButton(0))
            {
                _lastFakeTouch.phase = TouchPhase.Moved;
                Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                _lastFakeTouch.deltaPosition = newPosition - _lastFakeTouch.position;
                _lastFakeTouch.position = newPosition;
                _lastFakeTouch.fingerId = 0;
            }
            else
            {
                _lastFakeTouch = null;
            }
            if (_lastFakeTouch != null)
            {
                touches.Add(_lastFakeTouch.Create());
            }
            // Uncomment if you want it only to allow mouse swipes in the Unity Editor
            //#endif

            return touches;
        }
    }
}