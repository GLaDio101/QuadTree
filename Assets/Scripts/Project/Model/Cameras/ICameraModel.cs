using UnityEngine;

namespace Project.Model.Cameras
{
  public interface ICameraModel
  {
    void Add(string name, Camera camera);

    Camera GetByName(string name);

    void RemoveByName(string name);
  }
}