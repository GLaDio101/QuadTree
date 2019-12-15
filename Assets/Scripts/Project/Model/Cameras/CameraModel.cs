using System.Collections.Generic;
using UnityEngine;

namespace Project.Model.Cameras
{
  public class CameraModel : ICameraModel
  {
    private Dictionary<string, Camera> _map;

    [PostConstruct]
    public void OnPostConstruct()
    {
      _map = new Dictionary<string, Camera>();
    }

    public void Add(string name, Camera camera)
    {
      if (_map.ContainsKey(name))
        return;

      Debug.Log(name + " camera registered.");

      _map.Add(name, camera);
    }

    public Camera GetByName(string name)
    {
      if (_map.ContainsKey(name))
        return _map[name];

      return null;
    }

    public void RemoveByName(string name)
    {
      if (!_map.ContainsKey(name))
        return;

      _map.Remove(name);
    }
  }
}