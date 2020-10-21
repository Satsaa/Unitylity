

#if UNITY_EDITOR
namespace Muc.Editor.ReorderableLists {

  using System;
  using System.Collections.Generic;
  using UnityEditor;
  using UnityEngine;


  internal class ReorderableStructures : ReorderableValues {


    protected float idealLabelWidth;

    public ReorderableStructures(ReorderableAttribute attribute, SerializedProperty property, Type listType, Type elementType, bool editable) : base(attribute, property, listType, elementType, editable) { }

    //----------------------------------------------------------------------

    protected override float GetElementHeight(SerializedProperty element, int elementIndex) {
      var properties = element.EnumerateChildProperties();
      return GetElementHeight(properties);
    }

    protected float GetElementHeight(IEnumerable<SerializedProperty> properties) {
      var spacing = EditorGUIUtility.standardVerticalSpacing;
      var height = 0f;

      if (showElementHeader) {
        height += headerHeight + spacing;
      }

      idealLabelWidth = 0f;
      var labelStyle = EditorStyles.label;
      var labelContent = new GUIContent();

      var propertyCount = 0;
      foreach (var property in properties) {
        if (propertyCount++ > 0)
          height += spacing;

        height += GetPropertyHeight(property);

        labelContent.text = property.displayName;
        var minLabelWidth = labelStyle.CalcSize(labelContent).x;
        idealLabelWidth = Mathf.Max(idealLabelWidth, minLabelWidth);
      }
      idealLabelWidth += 8;
      return height;
    }

    //----------------------------------------------------------------------

    // original = 12; 15 is real indent width. Shifts elements so that expand arrows cannot overlap with the drag handle
    protected override float drawElementIndent => 0;

    protected override void DrawElement(Rect position, SerializedProperty element, int elementIndex, bool isActive) {
      var properties = element.EnumerateChildProperties();
      DrawElements(position, properties, elementIndex, isActive);
      if (elementIndex > 0)
        DrawHorizontalLine(position);
    }

    private static readonly GUIStyle eyeDropperHorizontalLine = "EyeDropperHorizontalLine";

    protected static void DrawHorizontalLine(Rect position) {
      if (IsRepaint()) {
        var style = eyeDropperHorizontalLine;
        position.yMin -= 3;
        position.height = 1;
        using (ColorScope(new Color(1, 1, 1, 0.75f))) {
          style.Draw(position, false, false, false, false);
        }
      }
    }

    protected override float borderHeight => 3;

    protected void DrawElements(Rect position, IEnumerable<SerializedProperty> properties, int elementIndex, bool isActive) {
      var spacing = EditorGUIUtility.standardVerticalSpacing;
      if (showElementHeader) {
        DrawElementHeader(position, elementIndex, isActive);
        position.y += headerHeight + spacing;
      }

      using (LabelWidthScope(EditorGUIUtility.labelWidth - position.xMin + 19)) {
        var propertyCount = 0;
        foreach (var property in properties) {
          if (propertyCount++ > 0)
            position.y += spacing;

          position.height = GetPropertyHeight(property);
          PropertyField(position, property);
          position.y += position.height;
        }
      }
    }

    //----------------------------------------------------------------------

    protected static readonly GUIStyle HeaderBackgroundStyle = "RL Header";

    private void DrawElementHeader(Rect position, int elementIndex, bool isActive) {
      position.xMin -= drawElementIndent;
      position.height = headerHeight;

      var titleContent = base.titleContent;

      titleContent.text = elementIndex.ToString();

      var titleStyle = EditorStyles.boldLabel;

      var titleWidth = titleStyle.CalcSize(titleContent).x;

      if (IsRepaint()) {
        var fillRect = position;
        fillRect.xMin -= draggable ? 18 : 4;
        fillRect.xMax += 4;
        fillRect.y -= 2;

        var fillStyle = HeaderBackgroundStyle;

        using (ColorAlphaScope(0.75f)) {
          fillStyle.Draw(fillRect, false, false, false, false);
        }

        var embossStyle = EditorStyles.whiteBoldLabel;
        var embossRect = position;
        embossRect.yMin -= 0;
        using (new EditorGUI.DisabledScope(true)) {
          embossStyle.Draw(embossRect, titleContent, false, false, false, false);
        }

        var titleRect = position;
        titleRect.yMin -= 1;
        titleRect.width = titleWidth;
        titleStyle.Draw(titleRect, titleContent, false, false, false, false);

        var menuRect = position;
        menuRect.xMin = menuRect.xMax - 16;
        menuRect.yMin += 4;
        ContextMenuButtonStyle.Draw(menuRect, false, false, false, false);
      }
    }

  }

}
#endif