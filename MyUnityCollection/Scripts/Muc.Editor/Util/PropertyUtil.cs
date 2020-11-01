#if UNITY_EDITOR
namespace Muc.Editor {

  using System.Collections;
  using System.Collections.Generic;
  using System.Reflection;
  using System.Text.RegularExpressions;
  using UnityEditor;
  using UnityEngine;


  public static class PropertyUtil {

    /// <summary>
    /// Get the FieldInfo of the serialized property.
    /// </summary>
    public static FieldInfo GetFieldInfo(SerializedProperty property) {
      string propertyPath = property.propertyPath;
      object value = property.serializedObject.targetObject;
      FieldInfo res = default;
      int cursor = 0;
      while (NextPathComponent(propertyPath, ref cursor, out var token)) {
        res = GetPathComponentFieldInfo(value, token);
        value = GetPathComponentValue(value, token);
      }
      return res;
    }

    /// <summary>
    /// Get the value of the serialized property.
    /// </summary>
    public static object GetValue(SerializedProperty property) {
      string propertyPath = property.propertyPath;
      object value = property.serializedObject.targetObject;
      int cursor = 0;
      while (NextPathComponent(propertyPath, ref cursor, out var token))
        value = GetPathComponentValue(value, token);
      return value;
    }

    /// <summary>
    /// Gets the values of the serialized property. Use to get values for multi-editing.
    /// </summary>
    public static IEnumerable<object> GetValues(SerializedProperty property) {
      string propertyPath = property.propertyPath;
      foreach (var targetObject in property.serializedObject.targetObjects) {
        object value = targetObject;
        int cursor = 0;
        while (NextPathComponent(propertyPath, ref cursor, out var token))
          value = GetPathComponentValue(value, token);
        yield return value;
      }
    }

    /// <summary>
    /// Set the value of the serialized property in all target Objects.
    /// </summary>
    public static void SetValue(SerializedProperty property, object value) {
      foreach (var targetObject in property.serializedObject.targetObjects) {
        Undo.RecordObject(property.serializedObject.targetObject, $"Set {property.name}");
        SetValueNoRecord(property, targetObject, value);
      }
      EditorUtility.SetDirty(property.serializedObject.targetObject);
      property.serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Set the value of the serialized property, but do not record the change.
    /// The change will not be persisted unless you call SetDirty and ApplyModifiedProperties.
    /// </summary>
    public static void SetValueNoRecord(SerializedProperty property, Object targetObject, object value) {
      string propertyPath = property.propertyPath;
      object container = property.serializedObject.targetObject;

      int cursor = 0;
      NextPathComponent(propertyPath, ref cursor, out var deferredToken);
      while (NextPathComponent(propertyPath, ref cursor, out var token)) {
        container = GetPathComponentValue(container, deferredToken);
        deferredToken = token;
      }
      Debug.Assert(!container.GetType().IsValueType, $"Cannot use SerializedObject.SetValue on a struct object, as the result will be set on a temporary.  Either change {container.GetType().Name} to a class, or use SetValue with a parent member.");
      SetPathComponentValue(container, deferredToken, value);
    }

    // Union type representing either a property name or array element index.  The element
    // index is valid only if propertyName is null.
    readonly struct PropertyPathComponent {
      public readonly string propertyName;
      public readonly int elementIndex;
      public PropertyPathComponent(string propertyName) : this() => this.propertyName = propertyName;
      public PropertyPathComponent(int elementIndex) : this() => this.elementIndex = elementIndex;
    }

    static Regex arrayElementRegex = new Regex(@"\GArray\.data\[(\d+)\]", RegexOptions.Compiled);

    // Parse the next path component from a SerializedProperty.propertyPath.  For simple field/property access,
    // this is just tokenizing on '.' and returning each field/property name.  Array/list access is via
    // the pseudo-property "Array.data[N]", so this method parses that and returns just the array/list index N.
    //
    // Call this method repeatedly to access all path components.  For example:
    //
    //      string propertyPath = "quests.Array.data[0].goal";
    //      int i = 0;
    //      NextPropertyPathToken(propertyPath, ref i, out var component);
    //          => component = { propertyName = "quests" };
    //      NextPropertyPathToken(propertyPath, ref i, out var component) 
    //          => component = { elementIndex = 0 };
    //      NextPropertyPathToken(propertyPath, ref i, out var component) 
    //          => component = { propertyName = "goal" };
    //      NextPropertyPathToken(propertyPath, ref i, out var component) 
    //          => returns false
    static bool NextPathComponent(string propertyPath, ref int cursor, out PropertyPathComponent component) {

      if (cursor >= propertyPath.Length) {
        component = default;
        return false;
      }

      var arrayElementMatch = arrayElementRegex.Match(propertyPath, cursor);
      if (arrayElementMatch.Success) {
        cursor += arrayElementMatch.Length + 1; // Skip past next '.'
        component = new PropertyPathComponent(int.Parse(arrayElementMatch.Groups[1].Value));
        return true;
      }

      int dot = propertyPath.IndexOf('.', cursor);
      if (dot == -1) {
        component = new PropertyPathComponent(propertyPath.Substring(cursor));
        cursor = propertyPath.Length;
      } else {
        component = new PropertyPathComponent(propertyPath.Substring(cursor, dot - cursor));
        cursor = dot + 1; // Skip past next '.'
      }

      return true;
    }

    static object GetPathComponentValue(object container, PropertyPathComponent component) {
      if (component.propertyName == null)
        return ((IList)container)[component.elementIndex];
      else
        return GetMemberValue(container, component.propertyName);
    }

    static void SetPathComponentValue(object container, PropertyPathComponent component, object value) {
      if (component.propertyName == null)
        ((IList)container)[component.elementIndex] = value;
      else
        SetMemberValue(container, component.propertyName, value);
    }

    static object GetMemberValue(object container, string name) {
      if (container == null)
        return null;
      var type = container.GetType();
      var members = type.GetMember(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
      for (int i = 0; i < members.Length; ++i) {
        if (members[i] is FieldInfo field)
          return field.GetValue(container);
        else if (members[i] is PropertyInfo property)
          return property.GetValue(container);
      }
      return null;
    }

    static void SetMemberValue(object container, string name, object value) {
      var type = container.GetType();
      var members = type.GetMember(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
      for (int i = 0; i < members.Length; ++i) {
        if (members[i] is FieldInfo field) {
          field.SetValue(container, value);
          return;
        } else if (members[i] is PropertyInfo property) {
          property.SetValue(container, value);
          return;
        }
      }
      Debug.LogError($"Failed to set member {container}.{name} via reflection");
    }

    static FieldInfo GetPathComponentFieldInfo(object container, PropertyPathComponent component) {
      if (component.propertyName == null)
        return null;
      else
        return container.GetType().GetField(component.propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
    }

  }
}
#endif