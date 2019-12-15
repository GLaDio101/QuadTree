using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using ScreenCapture = Core.Utils.ScreenCapture;

namespace Core.Editor.Release
{
  [Serializable]
  public class ScreenShotData
  {
    public int CurrentResolutionIndex;
  }

  public class CreateScreenshots : UnityEditor.Editor
  {
    private const string Path = "Screenshots";

    private static readonly List<Vector2> ResolutionList = new List<Vector2>()
    {
      new Vector2(2048, 2732),
      new Vector2(1242, 2208),
      new Vector2(1080, 1920),
      new Vector2(1125, 2436),
    };

    public static ScreenShotData Data = new ScreenShotData();

    [MenuItem("Tools/Take Screenshots #&%j", false, 300)]
    [UsedImplicitly]
    public static void CreateScreenshotsUtility()
    {
      if (!Directory.Exists(Path))
        Directory.CreateDirectory(Path);

      Data.CurrentResolutionIndex = 0;

      CaptureNextResolution();
    }

    public static void CaptureNextResolution()
    {
      if (ResolutionList.Count <= Data.CurrentResolutionIndex)
        return;

      var w = (int) ResolutionList[Data.CurrentResolutionIndex].x;
      var h = (int) ResolutionList[Data.CurrentResolutionIndex].y;
      ChangeScreenResoution(w, h);

      var capturerObject = GameObject.Find("Camera");

      if (capturerObject == null)
      {
        Debug.LogWarning("Object with name 'Capture'  not found.");
        return;
      }

      var capturer = capturerObject.GetComponent<ScreenCapture>();

      if (capturer == null)
      {
        Debug.LogWarning("ScreenCapture component not found.");
        return;
      }

      capturer.StartCapture(OnScreenCaptured);
    }

    private static void ChangeScreenResoution(int w, int h)
    {
      if (!GameViewUtils.SizeExists(GameViewSizeGroupType.Android, w, h))
      {
        GameViewUtils.AddCustomSize(GameViewUtils.GameViewSizeType.FixedResolution, GameViewSizeGroupType.Android, w, h,
          w + "x" + h);
      }

      var idx = GameViewUtils.FindSize(GameViewSizeGroupType.Android, w, h);
      if (idx != -1)
        GameViewUtils.SetSize(idx);
    }

    public static void OnScreenCaptured()
    {
      Data.CurrentResolutionIndex++;
      if (ResolutionList.Count > Data.CurrentResolutionIndex)
      {
        CaptureNextResolution();
      }
      else
      {
        Debug.Log("Screenshots created.");
      }
    }
  }
}