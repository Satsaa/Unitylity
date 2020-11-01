#if UNITY_EDITOR
namespace Muc.Editor {

  using System;
  using System.Reflection;
  using UnityEditor;
  using UnityEngine;


  public static class EditorUtil {

    #region Variables

    public const float indentSize = 15; // kIndentPerLevel
    public const float lineHeight = 18; // kSingleLineHeight
    public const float smallLineHeight = 16f; // kSingleSmallLineHeight

    public static float spacing => EditorGUIUtility.standardVerticalSpacing; // kControlVerticalSpacing

    public static int indentLevel => EditorGUI.indentLevel;
    public static float labelWidth => EditorGUIUtility.labelWidth;
    public static float fieldWidth => EditorGUIUtility.fieldWidth;

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


    #region Scopes

    public static Deferred DisabledScope() {
      var prev = GUI.enabled;
      return new Deferred(() => GUI.enabled = prev);
    }

    public static Deferred DisabledScope(Func<bool, bool> modifier) {
      var prev = GUI.enabled;
      GUI.enabled = !modifier(!prev);
      return new Deferred(() => GUI.enabled = prev);
    }


    public static Deferred LabelWidthScope() {
      var prev = EditorGUIUtility.labelWidth;
      return new Deferred(() => EditorGUIUtility.labelWidth = prev);
    }

    public static Deferred LabelWidthScope(Func<float, float> modifier) {
      var prev = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = modifier(prev);
      return new Deferred(() => EditorGUIUtility.labelWidth = prev);
    }


    public static Deferred FieldWidthScope() {
      var prev = EditorGUIUtility.fieldWidth;
      return new Deferred(() => EditorGUIUtility.fieldWidth = prev);
    }

    public static Deferred FieldWidthScope(Func<float, float> modifier) {
      var prev = EditorGUIUtility.fieldWidth;
      EditorGUIUtility.fieldWidth = modifier(prev);
      return new Deferred(() => EditorGUIUtility.fieldWidth = prev);
    }


    public static Deferred IndentScope() {
      var prev = EditorGUI.indentLevel;
      return new Deferred(() => EditorGUI.indentLevel = prev);
    }

    public static Deferred IndentScope(Func<int, int> modifier) {
      var prev = EditorGUI.indentLevel;
      EditorGUI.indentLevel = modifier(prev);
      return new Deferred(() => EditorGUI.indentLevel = prev);
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