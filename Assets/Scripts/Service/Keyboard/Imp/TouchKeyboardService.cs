using System;
using UnityEngine;

namespace Service.Keyboard.Imp
{
  public class TouchKeyboardService : IKeyboardService
  {
    public float Height
    {
      get
      {
#if UNITY_EDITOR
        return TouchScreenKeyboard.area.height;
#elif UNITY_IOS
        return TouchScreenKeyboard.area.height;
#elif UNITY_ANDROID
        return GetKeyboardSize();
#else
        return 0;
#endif
      }
    }

    public bool Visible
    {
      get { return Math.Abs(Height) > .1f; }
    }

#if UNITY_ANDROID
        public int GetKeyboardSize()
        {
            using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject View =
 UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

                using (AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
                {
                    View.Call("getWindowVisibleDisplayFrame", Rct);

                    return Screen.height - Rct.Call<int>("height");
                }
            }
        }
#endif
  }
}