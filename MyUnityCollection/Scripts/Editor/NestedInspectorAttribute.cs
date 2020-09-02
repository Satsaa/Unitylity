

// Original: https://github.com/Deadcows/MyBox

namespace Muc.Editor {

  using UnityEngine;

  /// <summary>
  /// Display a nested inspector of a serialized object
  /// </summary>
  public class NestedInspectorAttribute : PropertyAttribute {

    public bool displayScript;

    public NestedInspectorAttribute(bool displayScriptField = true) {
      displayScript = displayScriptField;
    }

  }
}


#if UNITY_EDITOR
namespace Muc.Editor {

  using UnityEngine;
  using UnityEditor;

  [CustomPropertyDrawer(typeof(NestedInspectorAttribute))]
  internal class NestedInspectorDrawer : PropertyDrawer {

    private NestedInspectorAttribute instance { get { return _instance ?? (_instance = attribute as NestedInspectorAttribute); } }
    private NestedInspectorAttribute _instance;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      if (instance.displayScript || property.objectReferenceValue == null) {
        position.height = EditorGUI.GetPropertyHeight(property);
        EditorGUI.PropertyField(position, property);
        position.y += EditorGUI.GetPropertyHeight(property) + 4;
      }

      if (property.objectReferenceValue != null) {
        var startY = position.y;
        float startX = position.x;

        var propertyObject = new SerializedObject(property.objectReferenceValue).GetIterator();
        propertyObject.Next(true);
        propertyObject.NextVisible(false);

        var xPos = position.x + 10;
        var width = position.width - 10;

        while (propertyObject.NextVisible(propertyObject.isExpanded)) {
          position.x = xPos + 10 * propertyObject.depth;
          position.width = width - 10 * propertyObject.depth;

          position.height = propertyObject.isExpanded ? 16 : EditorGUI.GetPropertyHeight(propertyObject);
          EditorGUI.PropertyField(position, propertyObject);
          position.y += propertyObject.isExpanded ? 20 : EditorGUI.GetPropertyHeight(propertyObject) + 4;
        }

        var bgRect = new Rect(position);
        bgRect.y = startY - 5;
        bgRect.x = startX - 10;
        bgRect.height = position.y - startY;
        bgRect.width = 10;
        DrawColouredRect(bgRect, new Color(0.6f, 0.6f, 0.8f, 0.5f));

        if (GUI.changed) propertyObject.serializedObject.ApplyModifiedProperties();
      }
      if (GUI.changed) property.serializedObject.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      if (property.objectReferenceValue == null) return base.GetPropertyHeight(property, label);

      float height = instance.displayScript ? EditorGUI.GetPropertyHeight(property) + 4 : 0;

      var propertyObject = new SerializedObject(property.objectReferenceValue).GetIterator();
      propertyObject.Next(true);
      propertyObject.NextVisible(true);

      while (propertyObject.NextVisible(propertyObject.isExpanded)) {
        height += propertyObject.isExpanded ? 20 : EditorGUI.GetPropertyHeight(propertyObject) + 4;
      }

      return height;
    }

    private void DrawColouredRect(Rect rect, Color color) {
      var defaultBackgroundColor = GUI.backgroundColor;
      GUI.backgroundColor = color;
      GUI.Box(rect, "");
      GUI.backgroundColor = defaultBackgroundColor;
    }
  }

}
#endif
