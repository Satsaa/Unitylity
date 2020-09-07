

#if UNITY_EDITOR
namespace Muc.Editor.ReorderableLists {

  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using UnityEditor;
  using UnityEditorInternal;
  using UnityEngine;


  [CustomPropertyDrawer(typeof(ReorderableAttribute))]
  public class ReorderableDrawer : ArrayDrawer {

    public delegate void ElementDelegate(SerializedProperty array, int index);

    public static event ElementDelegate onElementSelected;

    public struct ElementSelectionScope : IDisposable {
      private readonly ElementDelegate callback;

      public ElementSelectionScope(ElementDelegate callback) {
        this.callback = callback;
        onElementSelected += this.callback;
      }

      public void Dispose() {
        onElementSelected -= callback;
      }
    }

    //----------------------------------------------------------------------

    public delegate void BackgroundColorDelegate(SerializedProperty array, int index, ref Color backgroundColor);

    public static event BackgroundColorDelegate onBackgroundColor;

    public struct BackgroundColorScope : IDisposable {
      private readonly BackgroundColorDelegate callback;

      public BackgroundColorScope(BackgroundColorDelegate callback) {
        this.callback = callback;
        onBackgroundColor += this.callback;
      }

      public void Dispose() {
        onBackgroundColor -= callback;
      }
    }

    //----------------------------------------------------------------------

    private static readonly ReorderableAttribute defaultAttribute = new ReorderableAttribute();

    internal new ReorderableAttribute attribute => (ReorderableAttribute)base.attribute ?? defaultAttribute;

    public override bool CanCacheInspectorGUI(SerializedProperty property) {
      return true;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      var reorderableListOfValues = GetReorderableList(attribute, fieldInfo, property);

      Debug.Assert(reorderableListOfValues.serializedProperty.propertyPath == property.propertyPath);

      try {
        return reorderableListOfValues.GetHeight(label);
      } catch (Exception ex) {
        Debug.LogException(ex);
        return 0f;
      }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

      var reorderableList = GetReorderableList(attribute, fieldInfo, property);
      reorderableList.UpdateLabel(label);
      reorderableList.onBackgroundColor = onBackgroundColor;
      reorderableList.onSelectCallback += OnSelectCallback;
      reorderableList.DoGUI(position);
      reorderableList.onSelectCallback -= OnSelectCallback;
      reorderableList.onBackgroundColor = null;
    }

    //----------------------------------------------------------------------

    private void OnSelectCallback(ReorderableList list) {
      var array = list.serializedProperty;
      var index = list.index;
      if (onElementSelected != null)
        onElementSelected.Invoke(array, index);
    }

    //----------------------------------------------------------------------

    private class ReorderableListMap : Dictionary<string, ReorderableValues> {
      public ReorderableValues Find(string key) {
        var reorderableList = default(ReorderableValues);
        base.TryGetValue(key, out reorderableList);
        return reorderableList;
      }
    }

    private readonly ReorderableListMap reorderableListMap = new ReorderableListMap();

    private ReorderableValues mostRecentReorderableList;

    private string mostRecentPropertyPath;

    private ReorderableValues GetReorderableList(ReorderableAttribute attribute, FieldInfo fieldInfo, SerializedProperty property) {
      var propertyPath = property.propertyPath;

      if (mostRecentReorderableList != null) {
        if (mostRecentPropertyPath == propertyPath) {
          mostRecentReorderableList.serializedProperty = property;
          return mostRecentReorderableList;
        }
      }

      mostRecentReorderableList = reorderableListMap.Find(propertyPath);

      if (mostRecentReorderableList == null) {
        var reorderableList = CreateReorderableList(attribute, fieldInfo, property);

        reorderableListMap.Add(propertyPath, reorderableList);

        mostRecentReorderableList = reorderableList;
      } else {
        mostRecentReorderableList.serializedProperty = property;
      }

      mostRecentPropertyPath = propertyPath;

      return mostRecentReorderableList;
    }

    private ReorderableValues CreateReorderableList(ReorderableAttribute attribute, FieldInfo fieldInfo, SerializedProperty property) {
      var listType = fieldInfo.FieldType;

      var elementType = GetArrayOrListElementType(listType);

      var elementIsValue =
          elementType.IsEnum ||
          elementType.IsPrimitive ||
          elementType == typeof(string) ||
          elementType == typeof(Color) ||
          elementType == typeof(LayerMask) ||
          elementType == typeof(Vector2) ||
          elementType == typeof(Vector3) ||
          elementType == typeof(Vector4) ||
          elementType == typeof(Rect) ||
          elementType == typeof(AnimationCurve) ||
          elementType == typeof(Bounds) ||
          elementType == typeof(Gradient) ||
          elementType == typeof(Quaternion) ||
          elementType == typeof(Vector2Int) ||
          elementType == typeof(Vector3Int) ||
          elementType == typeof(RectInt) ||
          elementType == typeof(BoundsInt);

      if (elementIsValue) {
        return new ReorderableValues(attribute, property, listType, elementType);
      }

      var elementIsUnityEngineObject = typeof(UnityEngine.Object).IsAssignableFrom(elementType);

      if (elementIsUnityEngineObject) {
        var elementsAreSubassets =
            elementIsUnityEngineObject &&
            attribute != null &&
            attribute.elementsAreSubassets;

        if (elementsAreSubassets) {
          var assemblies = AppDomain.CurrentDomain.GetAssemblies();

          var types = assemblies.SelectMany(a => a.GetTypes());

          var subassetTypes =
            types.Where(
              t => !t.IsAbstract
                && !t.IsGenericTypeDefinition
                && elementType.IsAssignableFrom(t)
            ).ToArray();

          return new ReorderableSubassets(attribute, property, listType, elementType, subassetTypes);
        } else {
          return new ReorderableValues(attribute, property, listType, elementType);
        }
      }

      var elementPropertyDrawerType = GetDrawerTypeForType(elementType);
      if (elementPropertyDrawerType == null) {
        var elementIsStruct =
            elementType.IsValueType &&
            !elementType.IsEnum &&
            !elementType.IsPrimitive;

        var elementIsClass = elementType.IsClass;

        if (elementIsStruct || elementIsClass) {
          return new ReorderableStructures(attribute, property, listType, elementType);
        }
      }

      return new ReorderableValues(attribute, property, listType, elementType);

    }

    //======================================================================

    private delegate Type GetArrayOrListElementTypeDelegate(Type listType);

    private static readonly GetArrayOrListElementTypeDelegate GetArrayOrListElementType =
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

    private static readonly GetDrawerTypeForTypeDelegate GetDrawerTypeForType =
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