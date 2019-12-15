using Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Drawers
{
  [CustomPropertyDrawer(typeof(LayerAttribute))]
  class LayerAttributeEditor : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      property.intValue = EditorGUI.LayerField(position, label, property.intValue);
    }
  }
}