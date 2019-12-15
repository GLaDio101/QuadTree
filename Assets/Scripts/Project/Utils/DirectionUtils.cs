using UnityEngine;

namespace Project.Utils
{
  public class DirectionUtils
  {
    private static Vector3[] dirVectors =
    {
      new Vector3(1, 0, 0), new Vector3(1, 0, -1), new Vector3(0, 0, -1), new Vector3(-1, 0, -1), new Vector3(-1, 0, 0),
      new Vector3(-1, 0, 1), new Vector3(0, 0, 1), new Vector3(1, 0, 1)
    };

    public static Vector3 ToAngle(int direction)
    {
      return new Vector3(0, direction * 45 + 45, 0);
    }

    public static int ToDirection(Vector3 angle)
    {
      return Mathf.RoundToInt((angle.y - 45) / 45);
    }

    public static Vector3 DirToVector(int direction)
    {
      return dirVectors[direction];
    }

    public static Vector3 AngleToVector(Vector3 angle)
    {
      return dirVectors[ToDirection(angle)];
    }
  }
}