using Project.Model.Cameras;
using strange.extensions.mediation.impl;

namespace Project.GameEntity.Camera
{
  public class CameraMediator : EventMediator
  {
    [Inject]
    public CameraView view { get; set; }

    [Inject]
    public ICameraModel cameraModel { get; set; }

    public override void OnRegister()
    {
      cameraModel.Add(view.Name, GetComponent<UnityEngine.Camera>());
    }

    public override void OnRemove()
    {
      cameraModel.RemoveByName(view.Name);
    }
  }
}