

#if UNITY_EDITOR
namespace Muc.Editor.ReorderableLists {

  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using UnityEditor;
  using UnityEditorInternal;
  using UnityEngine;

  using static EditorUtil;


  public class ReorderableDrawer : ArrayDrawer {

    public delegate void BackgroundColorDelegate(SerializedProperty array, int index, ref Color backgroundColor);

    //======================================================================

    public override bool CanCacheInspectorGUI(SerializedProperty property) {
      return true;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      var reorderableListOfValues = GetReorderableList(fieldInfo, property);

      Debug.Assert(reorderableListOfValues.serializedProperty.propertyPath == property.propertyPath);

      try {
        return reorderableListOfValues.GetHeight(label);
      } catch (Exception ex) {
        Debug.LogException(ex);
        return 0f;
      }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      // Add property scope
      var propertyPos = position;
      propertyPos.height = 20 + (property.isExpanded ? 1 : 0);
      using (PropertyScope(propertyPos, label, property, out label)) {
        var reorderableList = GetReorderableList(fieldInfo, property);
        reorderableList.UpdateLabel(label);
        reorderableList.onSelectCallback += OnSelectCallback;
        reorderableList.DoGUI(position);
        reorderableList.onSelectCallback -= OnSelectCallback;
      }
    }

    //======================================================================

    private void OnSelectCallback(ReorderableList list) {

    }

    //======================================================================

    private class ReorderableListMap : Dictionary<string, ReorderableValues> {
      public ReorderableValues Find(string key) {
        base.TryGetValue(key, out ReorderableValues reorderableList);
        return reorderableList;
      }
    }

    private readonly ReorderableListMap reorderableListMap = new ReorderableListMap();
    private ReorderableValues mostRecentReorderableList;
    private string mostRecentPropertyPath;

    private ReorderableValues GetReorderableList(FieldInfo fieldInfo, SerializedProperty property) {
      var propertyPath = property.propertyPath;

      if (mostRecentReorderableList != null) {
        if (mostRecentPropertyPath == propertyPath) {
          mostRecentReorderableList.serializedProperty = property;
          return mostRecentReorderableList;
        }
      }

      mostRecentReorderableList = reorderableListMap.Find(propertyPath);

      if (mostRecentReorderableList == null) {
        var reorderableList = CreateReorderableList(fieldInfo, property);
        reorderableListMap.Add(propertyPath, reorderableList);
        mostRecentReorderableList = reorderableList;
      } else {
        mostRecentReorderableList.serializedProperty = property;
      }

      mostRecentPropertyPath = propertyPath;

      return mostRecentReorderableList;
    }

    private ReorderableValues CreateReorderableList(FieldInfo fieldInfo, SerializedProperty property) {
      var listType = fieldInfo.FieldType;

      var elementType = getArrayOrListElementType(listType);
      var elementIsValue = EditorUtil.TypeIsBasic(elementType);

      if (property.arrayElementType == "managedReference<>") {
        return new ReorderableReferences(property);
      }

      if (elementIsValue) {
        return new ReorderableValues(property);
      }

      var elementIsUnityEngineObject = typeof(UnityEngine.Object).IsAssignableFrom(elementType);

      if (elementIsUnityEngineObject) {
        return new ReorderableValues(property);
      }

      var elementPropertyDrawerType = getDrawerTypeForType(elementType);
      if (elementPropertyDrawerType == null) {
        var elementIsStruct =
            elementType.IsValueType &&
            !elementType.IsEnum &&
            !elementType.IsPrimitive;

        var elementIsClass = elementType.IsClass;

        if (elementIsStruct || elementIsClass) {
          return new ReorderableStructures(property);
        }
      }

      return new ReorderableValues(property);

    }

    //======================================================================

    private delegate Type GetArrayOrListElementTypeDelegate(Type listType);

    private static readonly GetArrayOrListElementTypeDelegate getArrayOrListElementType =
      (GetArrayOrListElementTypeDelegate)Delegate.CreateDelegate(
        typeof(GetArrayOrListElementTypeDelegate),
        null,
        typeof(PropertyDrawer)
          .Assembly
          .GetType("UnityEditor.EditorExtensionMethods")
          .GetMethod(
            "GetArrayOrListElementType",
            BindingFlags.NonPublic | BindingFlags.Static
          )
      );

    //======================================================================

    private delegate Type GetDrawerTypeForTypeDelegate(Type type);

    private static readonly GetDrawerTypeForTypeDelegate getDrawerTypeForType =
      (GetDrawerTypeForTypeDelegate)Delegate.CreateDelegate(
        typeof(GetDrawerTypeForTypeDelegate),
        null,
        typeof(PropertyDrawer)
          .Assembly
          .GetType("UnityEditor.ScriptAttributeUtility")
          .GetMethod(
            "GetDrawerTypeForType",
            BindingFlags.NonPublic | BindingFlags.Static
          )
      );

  }

}
#endif