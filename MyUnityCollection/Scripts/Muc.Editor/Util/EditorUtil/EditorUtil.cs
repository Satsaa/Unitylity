#if UNITY_EDITOR
namespace Muc.Editor {

  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using UnityEditor;
  using UnityEngine;


  public static partial class EditorUtil {

    #region Variables

    public const float indentSize = 15; // kIndentPerLevel
    public const float lineHeight = 18; // kSingleLineHeight
    public const float smallLineHeight = 16f; // kSingleSmallLineHeight

    public static float spacing => EditorGUIUtility.standardVerticalSpacing; // kControlVerticalSpacing

    public static int indentLevel { get => EditorGUI.indentLevel; set => EditorGUI.indentLevel = value; }
    public static float labelWidth { get => EditorGUIUtility.labelWidth; set => EditorGUIUtility.labelWidth = value; }
    public static float fieldWidth { get => EditorGUIUtility.fieldWidth; set => EditorGUIUtility.fieldWidth = value; }

    #endregion


    #region Rects

    public static Rect LabelRect() => LabelRect(EditorGUIUtility.singleLineHeight);
    public static Rect LabelRect(float height) {
      Rect res = default;
      res.xMin = indentLevel * indentSize;
      res.width = labelWidth;
      res.height = height;
      return res;
    }

    public static Rect FieldRect() => FieldRect(EditorGUIUtility.singleLineHeight);
    public static Rect FieldRect(float height) {
      Rect res = default;
      res.xMin = indentLevel * indentSize + labelWidth;
      res.width = fieldWidth;
      res.height = height;
      return res;
    }

    #endregion


    #region Utility

    /// <summary>
    /// Basic? I don't know what that means... simple Types that Unity renders on a single line.
    /// </summary>
    public static bool TypeIsBasic(Type type) {
      return type.IsEnum ||
        type.IsPrimitive ||
        type == typeof(string) ||
        type == typeof(Color) ||
        type == typeof(LayerMask) ||
        type == typeof(Vector2) ||
        type == typeof(Vector3) ||
        type == typeof(Vector4) ||
        type == typeof(Rect) ||
        type == typeof(AnimationCurve) ||
        type == typeof(Bounds) ||
        type == typeof(Gradient) ||
        type == typeof(Quaternion) ||
        type == typeof(Vector2Int) ||
        type == typeof(Vector3Int) ||
        type == typeof(RectInt) ||
        type == typeof(BoundsInt);
    }

    #endregion


    #region Scopes

    public static Deferred RestoreDisabledScope() {
      var prev = GUI.enabled;
      return new Deferred(() => GUI.enabled = prev);
    }

    /// <summary> <remarks> Does not enable if already disabled. </remarks> </summary>
    public static Deferred DisabledScope(bool disable = true) {
      var prev = GUI.enabled;
      GUI.enabled &= !disable;
      return new Deferred(() => GUI.enabled = prev);
    }

    public static Deferred DisabledScope(Func<bool, bool> modifier) {
      var prev = GUI.enabled;
      GUI.enabled = !modifier(!prev);
      return new Deferred(() => GUI.enabled = prev);
    }


    public static Deferred RestoreLabelWidthScope() {
      var prev = EditorGUIUtility.labelWidth;
      return new Deferred(() => EditorGUIUtility.labelWidth = prev);
    }

    /// <summary> <remarks> Setting width to 0 will reset it to the default value. </remarks> </summary>
    public static Deferred LabelWidthScope(float width) {
      var prev = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = width;
      return new Deferred(() => EditorGUIUtility.labelWidth = prev);
    }

    /// <summary> <remarks> Setting width to 0 will reset it to the default value. </remarks> </summary>
    public static Deferred LabelWidthScope(Func<float, float> modifier) {
      var prev = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = modifier(prev);
      return new Deferred(() => EditorGUIUtility.labelWidth = prev);
    }


    public static Deferred RestoreFieldWidthScope() {
      var prev = EditorGUIUtility.fieldWidth;
      return new Deferred(() => EditorGUIUtility.fieldWidth = prev);
    }

    /// <summary> <remarks> Setting width to 0 will reset it to the default value. </remarks> </summary>
    public static Deferred FieldWidthScope(float width) {
      var prev = EditorGUIUtility.fieldWidth;
      EditorGUIUtility.fieldWidth = width;
      return new Deferred(() => EditorGUIUtility.fieldWidth = prev);
    }

    /// <summary> <remarks> Setting width to 0 will reset it to the default value. </remarks> </summary>
    public static Deferred FieldWidthScope(Func<float, float> modifier) {
      var prev = EditorGUIUtility.fieldWidth;
      EditorGUIUtility.fieldWidth = modifier(prev);
      return new Deferred(() => EditorGUIUtility.fieldWidth = prev);
    }


    public static Deferred RestoreIndentScope() {
      var prev = EditorGUI.indentLevel;
      return new Deferred(() => EditorGUI.indentLevel = prev);
    }

    public static Deferred IndentScope(int additionIndentation = 1) {
      var prev = EditorGUI.indentLevel;
      EditorGUI.indentLevel += additionIndentation;
      return new Deferred(() => EditorGUI.indentLevel = prev);
    }

    public static Deferred IndentScope(Func<int, int> modifier) {
      var prev = EditorGUI.indentLevel;
      EditorGUI.indentLevel = modifier(prev);
      return new Deferred(() => EditorGUI.indentLevel = prev);
    }


    public static Deferred RestoreMixedValueScope() {
      var prev = EditorGUI.showMixedValue;
      return new Deferred(() => EditorGUI.showMixedValue = prev);
    }

    public static Deferred MixedValueScope(bool mixed = true) {
      var prev = EditorGUI.showMixedValue;
      EditorGUI.showMixedValue |= mixed;
      return new Deferred(() => EditorGUI.showMixedValue = prev);
    }

    public static Deferred MixedValueScope(Func<bool, bool> modifier) {
      var prev = EditorGUI.showMixedValue;
      EditorGUI.showMixedValue = modifier(prev);
      return new Deferred(() => EditorGUI.showMixedValue = prev);
    }


    public static Deferred RestoreBackgroundColorScope() {
      var prev = GUI.backgroundColor;
      return new Deferred(() => GUI.backgroundColor = prev);
    }

    public static Deferred BackgroundColorScope(Color color) {
      var prev = GUI.backgroundColor;
      GUI.backgroundColor = color;
      return new Deferred(() => GUI.backgroundColor = prev);
    }

    public static Deferred BackgroundColorScope(Func<Color, Color> modifier) {
      var prev = GUI.backgroundColor;
      GUI.backgroundColor = modifier(prev);
      return new Deferred(() => GUI.backgroundColor = prev);
    }


    public static Deferred RestoreColorScope() {
      var prev = GUI.color;
      return new Deferred(() => GUI.color = prev);
    }

    public static Deferred ColorScope(Color color) {
      var prev = GUI.color;
      GUI.color = color;
      return new Deferred(() => GUI.color = prev);
    }

    public static Deferred ColorScope(Func<Color, Color> modifier) {
      var prev = GUI.color;
      GUI.color = modifier(prev);
      return new Deferred(() => GUI.color = prev);
    }



    public static Deferred RestoreTextColorScope() {
      var prev = GUI.contentColor;
      return new Deferred(() => GUI.contentColor = prev);
    }

    public static Deferred TextColorScope(Color color) {
      var prev = GUI.contentColor;
      GUI.contentColor = color;
      return new Deferred(() => GUI.contentColor = prev);
    }

    public static Deferred TextColorScope(Func<Color, Color> modifier) {
      var prev = GUI.contentColor;
      GUI.contentColor = modifier(prev);
      return new Deferred(() => GUI.contentColor = prev);
    }



    public static Deferred PropertyScope(Rect totalPosition, GUIContent label, SerializedProperty property) {
      EditorGUI.BeginProperty(totalPosition, label, property);
      return new Deferred(() => EditorGUI.EndProperty());
    }

    public static Deferred PropertyScope(Rect totalPosition, GUIContent label, SerializedProperty property, out GUIContent newLabel) {
      newLabel = EditorGUI.BeginProperty(totalPosition, label, property);
      return new Deferred(() => EditorGUI.EndProperty());
    }


    public static Deferred HorizontalScope(params GUILayoutOption[] options) {
      EditorGUILayout.BeginHorizontal(options);
      return new Deferred(() => EditorGUILayout.EndHorizontal());
    }

    public static Deferred HorizontalScope(GUIStyle style, params GUILayoutOption[] options) {
      EditorGUILayout.BeginHorizontal(style, options);
      return new Deferred(() => EditorGUILayout.EndHorizontal());
    }


    public static Deferred VerticalScope(params GUILayoutOption[] options) {
      EditorGUILayout.BeginVertical(options);
      return new Deferred(() => EditorGUILayout.EndVertical());
    }

    public static Deferred VerticalScope(GUIStyle style, params GUILayoutOption[] options) {
      EditorGUILayout.BeginVertical(style, options);
      return new Deferred(() => EditorGUILayout.EndVertical());
    }

    #endregion

  }
}
#endif