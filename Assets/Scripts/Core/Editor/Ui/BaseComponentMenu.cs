using Core.Manager.Screen;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Editor.Ui
{
  public class BaseComponentMenu : UnityEditor.Editor
  {
    [MenuItem("GameObject/BrosUI/Root")]
    public static void CreateRoot()
    {
      GameObject mainRoot = new GameObject("Root");
      mainRoot.AddComponent<EventSystem>();
      mainRoot.AddComponent<StandaloneInputModule>();

      GameObject screenManager = new GameObject("ScreenManager");
      screenManager.transform.SetParent(mainRoot.transform);
      screenManager.AddComponent<RectTransform>();
      Canvas canvas = screenManager.AddComponent<Canvas>();
      canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      CanvasScaler canvasScaler = screenManager.AddComponent<CanvasScaler>();
      canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
      canvasScaler.referenceResolution = new Vector2(1920, 1080);
      canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
      canvasScaler.matchWidthOrHeight = .5f;
      canvasScaler.referencePixelsPerUnit = 32;
      screenManager.AddComponent<GraphicRaycaster>();

      ScreenManager smCom = screenManager.AddComponent<ScreenManager>();
      smCom.Layers = new Transform[5];
      for (int i = 0; i < 5; i++)
      {
        GameObject layer = new GameObject("Layer" + (i + 1));
        RectTransform scrt = layer.AddComponent<RectTransform>();
        MakeFullScreen(screenManager, scrt);
        smCom.Layers[i] = layer.transform;
      }

      GameObject camera = new GameObject("Camera");
      camera.transform.SetParent(mainRoot.transform);
      Camera cameraCom = camera.AddComponent<Camera>();
      cameraCom.allowMSAA = false;
      camera.AddComponent<FlareLayer>();
      camera.AddComponent<AudioListener>();

      GameObject audioManager = new GameObject("AudioManager");
      audioManager.transform.SetParent(mainRoot.transform);
//      audioManager.AddComponent<AudioManager>();
    }

    private static void MakeFullScreen(GameObject screenManager, RectTransform pcrt)
    {
      pcrt.SetParent(screenManager.transform);
      pcrt.anchorMax = Vector2.one;
      pcrt.anchorMin = Vector2.zero;
      pcrt.pivot = Vector2.one / 2;
      pcrt.offsetMin = Vector2.zero;
      pcrt.offsetMax = Vector2.zero;
    }
  }
}