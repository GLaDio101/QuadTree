using Core.Utils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Editor.Ui
{
    [CustomEditor(typeof(SwitchToggle))]
    public class SwitchToggleEditor : UnityEditor.Editor
    {
        [MenuItem("GameObject/BrosUI")]
        [MenuItem("GameObject/BrosUI/SwitchToggle")]
        public static void CreateSwitchToggle(MenuCommand menuCommand)
        {
            GameObject toggleRoot =
                UiComponentUtil.CreateUIElementRoot("SwitchToggle", menuCommand,
                    DefaultComponentStyle.s_ThickGUIElementSize);
            toggleRoot.transform.localPosition = Vector3.zero;
            toggleRoot.transform.localScale = Vector3.one;
            var toggleRootTransform = toggleRoot.GetComponent<RectTransform>();

            GameObject backgorund =
                UiComponentUtil.CreateUIElementRoot("Backgorund", menuCommand,
                    DefaultComponentStyle.s_ThickGUIElementSize);

            backgorund.transform.SetParent(toggleRoot.transform);
            var rectTransform = backgorund.GetComponent<RectTransform>();
            rectTransform.SetAnchor(AnchorPresets.StretchAll, 0, 0);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            backgorund.transform.localPosition = Vector3.zero;
            backgorund.transform.localScale = Vector3.one;

            GameObject ActiveCheck =
                UiComponentUtil.CreateUIElementRoot("Check", menuCommand,
                    DefaultComponentStyle.s_ThickGUIElementSize);

            ActiveCheck.transform.SetParent(backgorund.transform);
            ActiveCheck.transform.localScale = Vector3.one;
            var rectTransform1 = ActiveCheck.GetComponent<RectTransform>();
            rectTransform1.SetAnchor(AnchorPresets.MiddleLeft);
            rectTransform1.SetPivot(PivotPresets.MiddleLeft);
            rectTransform1.localPosition = Vector3.zero;

            rectTransform1.offsetMax = Vector2.zero;
            rectTransform1.offsetMin = Vector2.zero;
            rectTransform1.sizeDelta = new Vector2(toggleRootTransform.sizeDelta.x / 2, toggleRootTransform.sizeDelta.y);

            ActiveCheck.transform.localPosition = Vector3.zero;
//        rectTransform1.SetPivot(PivotPresets.MiddleCenter);

            GameObject DisableCheck =
                UiComponentUtil.CreateUIElementRoot("Check", menuCommand,
                    DefaultComponentStyle.s_ThickGUIElementSize);

            DisableCheck.transform.SetParent(backgorund.transform);
            DisableCheck.transform.localScale = Vector3.one;
            var rectTransform2 = DisableCheck.GetComponent<RectTransform>();
            rectTransform2.SetAnchor(AnchorPresets.MiddleRight);
            rectTransform2.SetPivot(PivotPresets.MiddleRight);
            rectTransform2.localPosition = Vector3.zero;
            rectTransform2.offsetMax = Vector2.zero;
            rectTransform2.offsetMin = Vector2.zero;
            rectTransform2.sizeDelta = new Vector2(toggleRootTransform.sizeDelta.x / 2, toggleRootTransform.sizeDelta.y);

            DisableCheck.transform.localPosition = Vector3.zero;
//        rectTransform2.SetPivot(PivotPresets.MiddleCenter);

            SwitchToggle toggle = toggleRoot.AddComponent<SwitchToggle>();

            var bgImage = backgorund.AddComponent<Image>();

            toggle.targetGraphic = bgImage;

            var checkActiveImage = ActiveCheck.AddComponent<Image>();
            checkActiveImage.color = Color.green;
            checkActiveImage.raycastTarget = false;
            var checkDisableImage = DisableCheck.AddComponent<Image>();
            checkDisableImage.color = Color.red;
            checkDisableImage.raycastTarget = false;

            toggle.ActiveImage = checkActiveImage;
            toggle.DisableImage = checkDisableImage;
        }

        private SwitchToggle myTarget;

        public override void OnInspectorGUI()
        {
            //setup


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            myTarget = (SwitchToggle) target;

            if (myTarget.ActiveImage != null)
            {
                ActiveSprite = myTarget.ActiveImage;
            }

            if (myTarget.DisableImage != null)
            {
                DisableSprite = myTarget.DisableImage;
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Active Toggle Image : ");
            EditorGUI.BeginChangeCheck();
            ActiveSprite = EditorGUILayout.ObjectField(ActiveSprite, typeof(Image), false) as Image;
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                myTarget.ActiveImage = _activeSprite;
                myTarget.ValueChanged(_isOn);
                EditorUtility.SetDirty(myTarget.ActiveImage);
                EditorSceneManager.MarkAllScenesDirty();
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Disable Toggle Image : ");
            EditorGUI.BeginChangeCheck();
            DisableSprite = EditorGUILayout.ObjectField(DisableSprite, typeof(Image), false) as Image;
            if (EditorGUI.EndChangeCheck())
            {
                myTarget.DisableImage = _disableSprite;
                myTarget.ValueChanged(_isOn);
                EditorUtility.SetDirty(myTarget.DisableImage);
                EditorSceneManager.MarkAllScenesDirty();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("IsOn: ");
            _isOn = myTarget.isOn;
            EditorGUI.BeginChangeCheck();
            _isOn = EditorGUILayout.Toggle(_isOn);
            if (EditorGUI.EndChangeCheck())
            {
                myTarget.isOn = _isOn;
                myTarget.ValueChanged(_isOn);
                EditorUtility.SetDirty(myTarget.DisableImage);
                EditorSceneManager.MarkAllScenesDirty();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private bool _isOn { get; set; }

        private Image _disableSprite;

        private Image DisableSprite
        {
            get { return _disableSprite; }
            set { _disableSprite = value; }
        }

        private Image ActiveSprite
        {
            get { return _activeSprite; }
            set { _activeSprite = value; }
        }

        private Image _activeSprite;
    }
}