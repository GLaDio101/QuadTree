using System;
using UnityEngine;

namespace Core.Editor.Code.ContextList
{
  [Serializable]
  public class ContextVo
  {
    [SerializeField, AppContext] public int Context;

    [HideInInspector] public bool visible;
  }
}