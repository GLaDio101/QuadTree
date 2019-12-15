using Core.Manager.Audio;
using Core.Manager.Screen;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Editor.Tools
{
    public class CreateRoot : UnityEditor.Editor
    {

        [MenuItem("GameObject/Create Root")]
        //[MenuItem("CONTEXT/GameObject/Create Root")]
        public static void CreateRootOperation()
        {
            GameObject mainRoot = new GameObject("MainRoot");

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
            screenManager.AddComponent<GraphicRaycaster>();

            GameObject screenContainer = new GameObject("ScreenContainer");
            RectTransform scrt = screenContainer.AddComponent<RectTransform>();
            MakFullScreen(screenManager, scrt);

            GameObject loadingIndicator = new GameObject("LoadingIndicator");
            loadingIndicator.SetActive(false);
            RectTransform lirt = loadingIndicator.AddComponent<RectTransform>();
            MakFullScreen(screenManager, lirt);
            loadingIndicator.AddComponent<CanvasRenderer>();
            Image lii = loadingIndicator.AddComponent<Image>();
            lii.color = new Color(0,0,0,0);

            GameObject loadingIcon = new GameObject("LoadingIcon");
            loadingIcon.transform.SetParent(loadingIndicator.transform);
            RectTransform licort = loadingIcon.AddComponent<RectTransform>();
            licort.anchoredPosition = Vector2.zero;
            loadingIcon.AddComponent<CanvasRenderer>();
            loadingIcon.AddComponent<Image>();

            GameObject panelContainer = new GameObject("PanelContainer");
            RectTransform pcrt = panelContainer.AddComponent<RectTransform>();
            MakFullScreen(screenManager, pcrt);

            GameObject camera = new GameObject("Camera");
            camera.transform.SetParent(mainRoot.transform);
            camera.AddComponent<Camera>();
            camera.AddComponent<FlareLayer>();
            camera.AddComponent<AudioListener>();


            GameObject audioManager = new GameObject("AudioManager");
            audioManager.transform.SetParent(mainRoot.transform);
            audioManager.AddComponent<AudioManager>();

            ScreenManager smCom = screenManager.AddComponent<ScreenManager>();
            smCom.Layers = new Transform[2];
            smCom.Layers[0] = screenContainer.transform;
            smCom.Layers[1] = panelContainer.transform;
            smCom.LoadingLayer = loadingIndicator;
        }

        private static void MakFullScreen(GameObject screenManager, RectTransform pcrt)
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
   