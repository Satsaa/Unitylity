

#if UNITY_EDITOR
namespace Muc.Editor.ReorderableLists {

  using System;
  using System.Reflection;
  using UnityEditor;
  using UnityEngine;


  internal class ArrayDrawerAdapter : PropertyDrawer {

    private new static readonly FieldInfo attribute = typeof(PropertyDrawer).GetField("m_Attribute", BindingFlags.NonPublic | BindingFlags.Instance);

    private new static readonly FieldInfo fieldInfo = typeof(PropertyDrawer).GetField("m_FieldInfo", BindingFlags.NonPublic | BindingFlags.Instance);

    private readonly ArrayDrawer arrayDrawer;

    internal ArrayDrawerAdapter(ArrayDrawer arrayDrawer) {
      attribute.SetValue(this, arrayDrawer.attribute);
      this.arrayDrawer = arrayDrawer;
    }

    //======================================================================

    public sealed override bool CanCacheInspectorGUI(SerializedProperty property) {
      ResolveFieldInfo(property);
      return arrayDrawer.CanCacheInspectorGUI(property);
    }

    public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      ResolveFieldInfo(property);
      return arrayDrawer.GetPropertyHeight(property, label);
    }

    public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      ResolveFieldInfo(property);
      arrayDrawer.OnGUI(position, property, label);
    }

    //======================================================================

    private void ResolveFieldInfo(SerializedProperty property) {
      if (arrayDrawer.fieldInfo == null) {
        var fieldInfo = GetFieldInfo(property);
        ArrayDrawerAdapter.fieldInfo.SetValue(this, fieldInfo);
        arrayDrawer.fieldInfo = fieldInfo;
      }
    }

    //======================================================================

    private delegate FieldInfo GetFieldInfoFromPropertyDelegate(SerializedProperty property, out Type propertyType);

    private static readonly GetFieldInfoFromPropertyDelegate getFieldInfoFromProperty =
      (GetFieldInfoFromPropertyDelegate)Delegate.CreateDelegate(
        typeof(GetFieldInfoFromPropertyDelegate),
        null,
        typeof(PropertyDrawer)
          .Assembly
          .GetType("UnityEditor.ScriptAttributeUtility")
          .GetMethod("GetFieldInfoFromProperty", BindingFlags.NonPublic | BindingFlags.Static)
      );

    internal static FieldInfo GetFieldInfo(SerializedProperty property) {
      var fieldInfo = getFieldInfoFromProperty(property, out Type propertyType);
      if (fieldInfo == null) Debug.LogFormat("GetFieldInfo({0}) == null", property.propertyPath);
      return fieldInfo;
    }

  }

}
#endif