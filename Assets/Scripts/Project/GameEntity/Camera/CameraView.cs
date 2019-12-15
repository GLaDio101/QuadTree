using strange.extensions.mediation.impl;
using UnityEngine;

namespace Project.GameEntity.Camera
{
  [RequireComponent(typeof(UnityEngine.Camera))]
  public class CameraView : EventView
  {
    public string Name;
  }
}