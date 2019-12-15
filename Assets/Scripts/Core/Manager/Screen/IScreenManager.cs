using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Core.Manager.Screen
{
  public interface IScreenManager
  {
    IEventDispatcher disp { get; }

    GameObject LoadingLayer { get; set; }

    Transform[] Layers { get; set; }

    PrefabLoadType LoadType { get; set; }

    string BundlePath { get; set; }

    void HideLoader();

    void ShowLoader();
  }
}