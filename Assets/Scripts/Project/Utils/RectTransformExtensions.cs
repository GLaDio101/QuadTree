using UnityEngine;

namespace Project.Utils
{
  public static class RectTransformExtensions
  {
    public static void SetPivot(this RectTransform source, SpriteAlignment preset)
    {
      switch (preset)
      {
        case (SpriteAlignment.TopLeft):
        {
          source.pivot = new Vector2(0, 1);
          break;
        }
        case (SpriteAlignment.TopCenter):
        {
          source.pivot = new Vector2(0.5f, 1);
          break;
        }
        case (SpriteAlignment.TopRight):
        {
          source.pivot = new Vector2(1, 1);
          break;
        }

        case (SpriteAlignment.LeftCenter):
        {
          source.pivot = new Vector2(0, 0.5f);
          break;
        }
        case (SpriteAlignment.Center):
        {
          source.pivot = new Vector2(0.5f, 0.5f);
          break;
        }
        case (SpriteAlignment.RightCenter):
        {
          source.pivot = new Vector2(1, 0.5f);
          break;
        }

        case (SpriteAlignment.BottomLeft):
        {
          source.pivot = new Vector2(0, 0);
          break;
        }
        case (SpriteAlignment.BottomCenter):
        {
          source.pivot = new Vector2(0.5f, 0);
          break;
        }
        case (SpriteAlignment.BottomRight):
        {
          source.pivot = new Vector2(1, 0);
          break;
        }
      }
    }
  }
}