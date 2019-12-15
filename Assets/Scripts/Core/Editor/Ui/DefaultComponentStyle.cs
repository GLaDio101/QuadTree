using UnityEngine;

namespace Core.Editor.Ui
{
  public class DefaultComponentStyle
  {
    public const string kUILayerName = "UI";
    public const float kWidth = 160f;
    public const float kThickHeight = 30f;
    public const float kThinHeight = 20f;
    public const string kStandardSpritePath = "UI/Skin/UISprite.psd";
    public const string kBackgroundSpriteResourcePath = "UI/Skin/Background.psd";
    public const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
    public const string kKnobPath = "UI/Skin/Knob.psd";
    public const string kCheckmarkPath = "UI/Skin/Checkmark.psd";

    public static Vector2 s_ThickGUIElementSize = new Vector2(kWidth, kThickHeight);
    public static Vector2 s_ThinGUIElementSize = new Vector2(kWidth, kThinHeight);
    public static Vector2 s_ImageGUIElementSize = new Vector2(100f, 100f);
    public static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
    public static Color s_PanelColor = new Color(1f, 1f, 1f, 0.392f);
    public static Color s_TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
    public static float s_FontSize = 18f;
  }
}