using UnityEngine;

namespace Standard_Assets.Core.CompileTimeTracker.Editor.Extensions {
  public static class Vector2Extensions {
    public static Vector2 AddX(this Vector2 v, float x) {
      v.x = v.x + x;
      return v;
    }
  }
}
