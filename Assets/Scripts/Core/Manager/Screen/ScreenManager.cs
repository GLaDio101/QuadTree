using Project.Enums;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Core.Manager.Screen
{
    public class ScreenManager : EventView
    {
        public GameObject LoadingLayer;

        public Transform[] Layers;

        public PrefabLoadType LoadType;

        public string BundlePath;

        public void HideLoader()
        {
            if (LoadingLayer == null)
                return;

            LoadingLayer.SetActive(false);
        }

        public void ShowLoader()
        {
            if (LoadingLayer == null)
                return;

            LoadingLayer.SetActive(true);
        }
    }
}