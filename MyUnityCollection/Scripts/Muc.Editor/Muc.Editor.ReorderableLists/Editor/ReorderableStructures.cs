

#if UNITY_EDITOR
namespace Muc.Editor.ReorderableLists {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using UnityEditor;
  using UnityEngine;

  using static EditorUtil;
  using static PropertyUtil;


  internal class ReorderableStructures : ReorderableValues {


    public ReorderableStructures(SerializedProperty property) : base(property) { }

    //======================================================================

    protected override float GetElementHeight(SerializedProperty element, int elementIndex) {
      var properties = element.EnumerateChildProperties();
      var height = 0f;

      foreach (var property in properties) {
        height += EditorGUI.GetPropertyHeight(property);
      }
      if (elementIndex > 0) height += extraSpacing;
      return Mathf.Max(lineHeight, height);
    }

    //======================================================================

    protected override void DrawElement(Rect position, SerializedProperty element, int elementIndex, bool isActive) {
      if (elementIndex > 0) {
        DrawHorizontalLine(position);
        position.yMax += extraSpacing;
      } else {
        position.yMin -= extraSpacing;
      }
      DrawChildren(position, element);
    }

    private static readonly GUIStyle eyeDropperHorizontalLine = "EyeDropperHorizontalLine";

    protected void DrawHorizontalLine(Rect position) {
      if (Event.current.type == EventType.Repaint) {
        var style = eyeDropperHorizontalLine;
        position.yMin -= extraSpacing;
        position.xMin -= indentSize;
        position.height = 1;
        style.Draw(position, false, false, false, false);
      }
    }

    protected override float extraSpacing => 3;

    protected void DrawChildren(Rect position, SerializedProperty element) {
      var properties = element.EnumerateChildProperties();
      using (LabelWidthScope(v => v - position.xMin + 19)) {
        var propertyCount = 0;
        foreach (var property in properties) {
          if (propertyCount++ > 0)
            position.y += spacing;

          position.height = EditorGUI.GetPropertyHeight(property);
          EditorGUI.PropertyField(position, property);
          position.y += position.height;
        }
      }
    }

  }

}
#endif