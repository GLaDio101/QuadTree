using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Editor.Ui
{
  public class ButtonComponentMenu : UnityEditor.Editor
  {
    [MenuItem("GameObject/BrosUI")]
    [MenuItem("GameObject/BrosUI/Button/Image Button")]
    public static void CreateImageButton(MenuCommand menuCommand)
    {
      GameObject buttonRoot =
        UiComponentUtil.CreateUIElementRoot("ImageButton", menuCommand, DefaultComponentStyle.s_ThickGUIElementSize);

      Image image = buttonRoot.AddComponent<Image>();
      image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(DefaultComponentStyle.kStandardSpritePath);
      image.type = Image.Type.Sliced;
      image.color = DefaultComponentStyle.s_DefaultSelectableColor;

      Button bt = buttonRoot.AddComponent<Button>();
      UiComponentUtil.SetDefaultColorTransitionValues(bt);
    }

//    [MenuItem("GameObject/BrosUI/Button/Double Image Button")]
    public static void CreateDoubleImageButton(MenuCommand menuCommand)
    {
      GameObject buttonRoot =
        UiComponentUtil.CreateUIElementRoot("DoubleImageButton", menuCommand,
          DefaultComponentStyle.s_ImageGUIElementSize);

      GameObject childIcon = new GameObject("Icon");
      GameObjectUtility.SetParentAndAlign(childIcon, buttonRoot);

      Image image = buttonRoot.AddComponent<Image>();
      image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(DefaultComponentStyle.kStandardSpritePath);
      image.type = Image.Type.Sliced;
      image.color = DefaultComponentStyle.s_DefaultSelectableColor;

      Image icon = childIcon.AddComponent<Image>();
      icon.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(DefaultComponentStyle.kStandardSpritePath);
      icon.type = Image.Type.Simple;
      icon.color = DefaultComponentStyle.s_DefaultSelectableColor;

      Button bt = buttonRoot.AddComponent<Button>();
      UiComponentUtil.SetDefaultColorTransitionValues(bt);

      RectTransform iconRectTransform = childIcon.GetComponent<RectTransform>();
      iconRectTransform.anchorMin = Vector2.zero;
      iconRectTransform.anchorMax = Vector2.one;
      iconRectTransform.sizeDelta = Vector2.zero;
    }

//    [MenuItem("GameObject/BrosUI/Button/Text Button")]
    public static void CreateTextButton(MenuCommand menuCommand)
    {
      GameObject buttonRoot =
        UiComponentUtil.CreateUIElementRoot("TextButton", menuCommand, DefaultComponentStyle.s_ThickGUIElementSize);

      GameObject childText = new GameObject("Label");
      GameObjectUtility.SetParentAndAlign(childText, buttonRoot);

      Image image = buttonRoot.AddComponent<Image>();
      image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(DefaultComponentStyle.kStandardSpritePath);
      image.type = Image.Type.Sliced;
      image.color = DefaultComponentStyle.s_DefaultSelectableColor;

      Button bt = buttonRoot.AddComponent<Button>();
      UiComponentUtil.SetDefaultColorTransitionValues(bt);

      TextMeshProUGUI text = childText.AddComponent<TextMeshProUGUI>();
      text.text = "Button";
      text.alignment = TextAlignmentOptions.Midline;
      UiComponentUtil.SetDefaultTextValues(text);

      RectTransform textRectTransform = childText.GetComponent<RectTransform>();
      textRectTransform.anchorMin = Vector2.zero;
      textRectTransform.anchorMax = Vector2.one;
      textRectTransform.sizeDelta = Vector2.zero;
    }

//    [MenuItem("GameObject/BrosUI/Button/Text Button with Icon")]
    public static void CreateTextButtonWithIcon(MenuCommand menuCommand)
    {
      GameObject buttonRoot =
        UiComponentUtil.CreateUIElementRoot("TextWithIconButton ", menuCommand,
          DefaultComponentStyle.s_ThickGUIElementSize);

      GameObject childIcon = new GameObject("Icon");
      GameObjectUtility.SetParentAndAlign(childIcon, buttonRoot);

      GameObject childText = new GameObject("Label");
      GameObjectUtility.SetParentAndAlign(childText, buttonRoot);

      Image image = buttonRoot.AddComponent<Image>();
      image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(DefaultComponentStyle.kStandardSpritePath);
      image.type = Image.Type.Sliced;
      image.color = DefaultComponentStyle.s_DefaultSelectableColor;

      Image icon = childIcon.AddComponent<Image>();
      icon.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(DefaultComponentStyle.kStandardSpritePath);
      icon.type = Image.Type.Simple;
      icon.color = DefaultComponentStyle.s_DefaultSelectableColor;

      Button bt = buttonRoot.AddComponent<Button>();
      UiComponentUtil.SetDefaultColorTransitionValues(bt);

      TextMeshProUGUI text = childText.AddComponent<TextMeshProUGUI>();
      text.text = "Button";
      text.alignment = TextAlignmentOptions.Midline;
      UiComponentUtil.SetDefaultTextValues(text);

      RectTransform textRectTransform = childText.GetComponent<RectTransform>();
      textRectTransform.anchorMin = new Vector2(.2f, 0);
      textRectTransform.anchorMax = Vector2.one;
      textRectTransform.sizeDelta = Vector2.zero;

      RectTransform iconRectTransform = childIcon.GetComponent<RectTransform>();
      iconRectTransform.anchorMin = Vector2.zero;
      iconRectTransform.anchorMax = new Vector2(.2f, 1);
      iconRectTransform.sizeDelta = Vector2.zero;
    }

//    [MenuItem("GameObject/BrosUI/Button/Text Only Button")]
    public static void CreateTextOnlyButton(MenuCommand menuCommand)
    {
      GameObject buttonRoot =
        UiComponentUtil.CreateUIElementRoot("TextOnlyButton", menuCommand, DefaultComponentStyle.s_ThickGUIElementSize);

      TextMeshProUGUI text = buttonRoot.AddComponent<TextMeshProUGUI>();
      text.text = "Button";
      text.alignment = TextAlignmentOptions.Midline;
      UiComponentUtil.SetDefaultTextValues(text);

      Button bt = buttonRoot.AddComponent<Button>();
      UiComponentUtil.SetDefaultColorTransitionValues(bt);
    }
  }
}