// Developed by Tom Kail at Inkle
// Released under the MIT Licence as held at https://opensource.org/licenses/MIT

// Original: http://answers.unity.com/answers/1513032/view.html

namespace Muc.Editor {

  using UnityEngine;

  public class ShowEditorAttribute : PropertyAttribute {

    private readonly bool readOnly;

    public ShowEditorAttribute(bool readOnly = false) {
      this.readOnly = readOnly;
    }
  }
}


#if UNITY_EDITOR
namespace Muc.Editor {

  using System;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEditor;
  using System.Linq;
  using Object = UnityEngine.Object;
  using static PropertyUtil;

  /// <summary>
  /// Extends how ScriptableObject object references are displayed in the inspector
  /// Shows you all values under the object reference
  /// </summary>
  [CustomPropertyDrawer(typeof(ShowEditorAttribute), true)]
  public class ShowEditorAttributeDrawer : PropertyDrawer {

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      float totalHeight = EditorGUIUtility.singleLineHeight;
      if (property.objectReferenceValue == null || !AreAnySubPropertiesVisible(property)) {
        return totalHeight;
      }
      if (property.isExpanded) {
        var data = property.objectReferenceValue as ScriptableObject;
        if (data == null) return EditorGUIUtility.singleLineHeight;
        SerializedObject serializedObject = new SerializedObject(data);
        SerializedProperty prop = serializedObject.GetIterator();
        if (prop.NextVisible(true)) {
          do {
            if (prop.name == "m_Script") continue;
            var subProp = serializedObject.FindProperty(prop.name);
            float height = EditorGUI.GetPropertyHeight(subProp, null, true) + EditorGUIUtility.standardVerticalSpacing;
            totalHeight += height;
          }
          while (prop.NextVisible(false));
        }
        // Add a tiny bit of height if open for the background
        totalHeight += EditorGUIUtility.standardVerticalSpacing;
      }
      return totalHeight;
    }

    static readonly List<string> ignoreClassFullNames = new List<string> { "TMPro.TMP_FontAsset" };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      using (new EditorGUI.PropertyScope(position, label, property)) {

        var propertyRect = Rect.zero;
        var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
        if (property.objectReferenceValue != null && AreAnySubPropertiesVisible(property)) {
          property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, property.displayName, true);
        } else {
          foldoutRect.x += 10;
          EditorGUI.Foldout(foldoutRect, property.isExpanded, property.displayName, true, EditorStyles.label);
        }

        using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
          propertyRect = new Rect(position.x + EditorGUIUtility.labelWidth + 2, position.y, position.width - EditorGUIUtility.labelWidth - 2, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(propertyRect, property, GUIContent.none);
        if (GUI.changed) property.serializedObject.ApplyModifiedProperties();

        if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null) {

          if (property.isExpanded) {
            // Draw a background that shows us clearly which fields are part of the ScriptableObject
            GUI.Box(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, Screen.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");

            using (new EditorGUI.IndentLevelScope()) {

              SerializedObject serializedObject = new SerializedObject(GetValues(property).Cast<Object>().ToArray());

              // Iterate over all the values and draw them
              SerializedProperty prop = serializedObject.GetIterator();
              float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
              if (prop.NextVisible(true)) {
                do {
                  // Don't bother drawing the class file
                  if (prop.name == "m_Script") continue;
                  float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
                  EditorGUI.PropertyField(new Rect(position.x, y, position.width, height), prop, true);
                  y += height + EditorGUIUtility.standardVerticalSpacing;
                }
                while (prop.NextVisible(false));
              }
              if (GUI.changed) serializedObject.ApplyModifiedProperties();
            }
          }
        }
        property.serializedObject.ApplyModifiedProperties();
      }
    }

    static bool AreAnySubPropertiesVisible(SerializedProperty property) {
      var data = (ScriptableObject)property.objectReferenceValue;
      SerializedObject serializedObject = new SerializedObject(data);
      SerializedProperty prop = serializedObject.GetIterator();
      while (prop.NextVisible(true)) {
        if (prop.name == "m_Script") continue;
        return true; //if theres any visible property other than m_script
      }
      return false;
    }
  }
}
#endif