using System;
using System.Collections;
using System.Collections.Generic;
using Core.Manager.Bundle;
using JetBrains.Annotations;
using Project.View.Confirm;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Core.Manager.Screen
{
    public enum ScreenManagerEvent
    {
        OpenLogPanel
    }

    public class ScreenManagerMediator : EventMediator
    {
        [Inject] public ScreenManager view { get; set; }

        [Inject] public IScreenModel screenModel { get; set; }

        [Inject] public IBundleModel bundleModel { get; set; }

        private List<GameObject> _panels;

        private AssetBundle _bundle;

        public override void OnRegister()
        {
            view.dispatcher.AddListener(ScreenManagerEvent.OpenLogPanel, OnOpenPanel);

            dispatcher.AddListener(ScreenEvent.OpenPanel, OnOpenPanel);
            dispatcher.AddListener(ScreenEvent.Back, OnBack);
            dispatcher.AddListener(ScreenEvent.ShowLoader, OnShowLoader);
            dispatcher.AddListener(ScreenEvent.HideLoader, OnHideLoader);
            dispatcher.AddListener(ScreenEvent.ShowError, OnShowError);
            dispatcher.AddListener(ScreenEvent.Initialize, OnInitialize);

            _panels = new List<GameObject>();
            foreach (Transform layer in view.Layers)
            {
                foreach (Transform panel in layer)
                {
                    _panels.Add(panel.gameObject);
                }
            }
        }

        private void OnInitialize(IEvent payload)
        {
#if !UNITY_EDITOR
      view.LoadType = PrefabLoadType.Bundle;
#endif

            if (view.LoadType == PrefabLoadType.Bundle)
                bundleModel.LoadBundle(view.BundlePath).Done(OnBundleLoaded);
        }

        private void OnShowError(IEvent payload)
        {
            var message = (string) payload.data;

            if (string.IsNullOrEmpty(message) || message == "")
            {
                message = "Something went wrong :(";
            }

            ConfirmPanelVo panelVo = new ConfirmPanelVo
            {
                Name = "AlertPanel",
                Title = "LoginError",
                Description = message,
                ButtonLabel = "Ok",
                LayerIndex = 1
            };

            OpenPanelInner(panelVo);
        }

        private void OnBundleLoaded(BundleLoadData payload)
        {
            BundleLoadData bundleData = payload;
            if (bundleData != null && bundleData.Name == view.BundlePath)
            {
                _bundle = bundleData.Bundle;
            }
        }

        private void OnBack(IEvent payload)
        {
            if (screenModel.History.Count < 2)
                return;

            screenModel.History.RemoveAt(screenModel.History.Count - 1);
            IPanelVo prePanelVo = screenModel.History[screenModel.History.Count - 1];
            screenModel.History.RemoveAt(screenModel.History.Count - 1);

            dispatcher.Dispatch(ScreenEvent.OpenPanel, prePanelVo);
        }

        private void OnOpenPanel(IEvent payload)
        {
            if (payload == null)
                return;

            IPanelVo next = (IPanelVo) payload.data;
            OpenPanelInner(next);
        }

        private void OpenPanelInner(IPanelVo panelVo)
        {
            if (panelVo == null)
            {
                Debug.LogError("You have to send IPanelVo!!");
                return;
            }

            if (panelVo.LayerIndex >= view.Layers.Length)
            {
                Debug.LogError("There is no layer " + panelVo.LayerIndex);
                return;
            }

            // remove panels with the same name from other layers
//      foreach (Transform child in view.Layers[panelVo.LayerIndex].transform)
//      {
//        int index = child.name.IndexOf(panelVo.Name, StringComparison.Ordinal);
//        if (index != -1)
//        {
//          Destroy(child.gameObject);
//          RemoveLastByName(panelVo);
//        }
//      }

            CreateNewPanel(panelVo);
        }

        private void RemoveLastByName(IPanelVo panelVo)
        {
            for (int i = screenModel.History.Count - 1; i > 0; i--)
            {
                if (panelVo.Name == screenModel.History[i].Name)
                {
                    screenModel.History.RemoveAt(i);
                    return;
                }
            }
        }

        private void CreateNewPanel(IPanelVo vo)
        {
            switch (view.LoadType)
            {
                case PrefabLoadType.Resources:
                    StartCoroutine(LoadFromResources(vo));
                    break;
                case PrefabLoadType.Bundle:
                    LoadFromBundle(vo);
                    break;
                case PrefabLoadType.AssetDatabase:
                    LoadFromAssetDatabase(vo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LoadFromAssetDatabase(IPanelVo vo)
        {
#if UNITY_EDITOR
            GameObject template =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Panels/" + vo.Name + ".prefab");
            if (template == null)
            {
                Debug.LogError("Panel not found!! " + vo.Name);
                return;
            }

            CreatePanel(vo, template);
#endif
        }

        private IEnumerator LoadFromResources(IPanelVo vo)
        {
            ResourceRequest request = Resources.LoadAsync(vo.Name, typeof(GameObject));
            yield return request;
            if (request.asset == null)
            {
                Debug.LogError("Panel not found!! " + vo.Name);
                yield break;
            }

            CreatePanel(vo, request.asset as GameObject);
        }

        private void LoadFromBundle(IPanelVo vo)
        {
            GameObject template = _bundle.LoadAsset<GameObject>(vo.Name);
            if (template == null)
            {
                Debug.LogError("Panel not found!! " + vo.Name);
                return;
            }

            CreatePanel(vo, template);
        }

        private void CreatePanel(IPanelVo vo, GameObject template)
        {
            foreach (Transform child in view.Layers[vo.LayerIndex].transform)
            {
                int index = child.name.IndexOf(vo.Name, StringComparison.Ordinal);
                if (index != -1)
                {
                    Destroy(child.gameObject);
                    RemoveLastByName(vo);
                }
            }

            if (vo.RemoveLayer)
                CleanLayer(vo.LayerIndex);
            
            if (!screenModel.IgnoreHistory.Contains(vo.Name))
                screenModel.History.Add(vo);
            GameObject newPanel = Instantiate(template);

            IPanelView panelView = newPanel.GetComponent<IPanelView>();
            if (panelView != null)
                panelView.vo = vo;
            newPanel.transform.SetParent(view.Layers[vo.LayerIndex], false);
            newPanel.transform.localScale = Vector3.one;

            if (vo.RemoveAll)
                Clean();


            _panels.Add(newPanel);
        }

        private void CleanLayer(int voLayerIndex)
        {
            foreach (Transform panel in view.Layers[voLayerIndex].transform)
            {
                Destroy(panel.gameObject);
                _panels.Remove(panel.gameObject);
            }
        }

        private void OnShowLoader(IEvent payload)
        {
            if (payload.data != null)
            {
                int time = (int) payload.data;
                if (time > 0)
                {
                    StartCoroutine(WaitForTimeOut(time));
                }
            }

            view.ShowLoader();
        }

        private IEnumerator WaitForTimeOut(int time)
        {
            yield return new WaitForSeconds(time);
            view.HideLoader();
        }

        private void OnHideLoader(IEvent payload)
        {
            view.HideLoader();
        }

        private void Clean()
        {
            // clear open panels
            foreach (var panel in _panels)
            {
                Destroy(panel);
            }

            _panels.Clear();
        }

        [UsedImplicitly]
        private void OnApplicationQuit()
        {
            if (dispatcher != null)
                dispatcher.Dispatch(ScreenEvent.AppQuit);
        }

        [UsedImplicitly]
        private void OnApplicationFocus(bool hasFocus)
        {
            if (dispatcher != null)
                dispatcher.Dispatch(ScreenEvent.FocusChanged, hasFocus);
        }

        [UsedImplicitly]
        private void OnApplicationPause(bool pauseStatus)
        {
            if (dispatcher != null)
                dispatcher.Dispatch(ScreenEvent.PauseChanged, pauseStatus);
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(ScreenManagerEvent.OpenLogPanel, OnOpenPanel);
            dispatcher.RemoveListener(ScreenEvent.ShowError, OnShowError);
            dispatcher.RemoveListener(ScreenEvent.Back, OnBack);
            dispatcher.RemoveListener(ScreenEvent.OpenPanel, OnOpenPanel);
            dispatcher.RemoveListener(ScreenEvent.ShowLoader, OnShowLoader);
            dispatcher.RemoveListener(ScreenEvent.HideLoader, OnHideLoader);
            dispatcher.RemoveListener(ScreenEvent.Initialize, OnInitialize);
        }
    }
}