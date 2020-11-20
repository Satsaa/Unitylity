

#if UNITY_EDITOR
namespace Muc.Editor.ReorderableLists {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Text;
  using System.Text.RegularExpressions;
  using UnityEditor;
  using UnityEngine;


  internal static class SerializedPropertyUtility {

    public static IEnumerable<SerializedProperty> EnumerateChildProperties(this SerializedObject serializedObject) {
      var iterator = serializedObject.GetIterator();
      if (iterator.NextVisible(enterChildren: true)) {
        // yield return property; // skip "m_Script"
        while (iterator.NextVisible(enterChildren: false)) {
          yield return iterator;
        }
      }
    }

    public static IEnumerable<SerializedProperty> EnumerateChildProperties(this SerializedProperty property) {
      var iterator = property.Copy();
      SerializedProperty end;
      try { // GetEndProperty throws when ManagedReference arrays are different during multi edit
        end = iterator.GetEndProperty();
      } catch (InvalidOperationException) {
        yield break;
      }
      if (iterator.NextVisible(enterChildren: true)) {
        do {
          if (SerializedProperty.EqualContents(iterator, end))
            yield break;

          yield return iterator;
        }
        while (iterator.NextVisible(enterChildren: false));
      }
    }

    //======================================================================

    public static object FindObject(this object obj, IEnumerable<object> path) {
      foreach (var key in path) {
        if (key is string stringKey) {
          var objType = obj.GetType();
          var fieldName = stringKey;
          var fieldInfo = objType.FindFieldInfo(fieldName);
          if (fieldInfo == null)
            throw FieldNotFoundException(objType, fieldName);
          obj = fieldInfo.GetValue(obj);
          continue;
        }
        if (key is int intKey) {
          var elementIndex = intKey;
          var array = (IList)obj;
          obj = array[elementIndex];
          continue;
        }
      }
      return obj;
    }

    public static object GetObject(this SerializedProperty property) {
      var obj = property.serializedObject.targetObject;
      var path = ParseValuePath(property);
      return FindObject(obj, path);
    }

    private static Exception FieldNotFoundException(Type type, string fieldName) {
      return new KeyNotFoundException($"{type}.{fieldName} not found");
    }

    //======================================================================

    private static FieldInfo FindFieldInfo(this Type type, string fieldName) {
      const BindingFlags bindingFlags = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
      var fieldInfo = type.GetField(fieldName, bindingFlags);
      if (fieldInfo != null)
        return fieldInfo;

      var baseType = type.BaseType;
      if (baseType == null)
        return null;

      return FindFieldInfo(baseType, fieldName);
    }

    //======================================================================

    private static string GetValuePath(SerializedProperty property) {
      return property.propertyPath.Replace(".Array.data[", "[");
    }

    private static IEnumerable<object> ParseValuePath(SerializedProperty property) {
      return ParseValuePath(GetValuePath(property));
    }

    private static IEnumerable<object> ParseValuePath(string fieldPath) {
      var keys = fieldPath.Split('.');
      foreach (var key in keys) {
        if (key.IsElementIdentifier()) {
          var subkeys = key.Split('[', ']');
          yield return subkeys[0];
          foreach (var subkey in subkeys.Skip(1)) {
            if (string.IsNullOrEmpty(subkey)) {
              continue;
            }
            int index = int.Parse(subkey);
            yield return index;
          }
          continue;
        }
        if (key.IsElementIndex()) {
          var subkeys = key.Split('[', ']');
          foreach (var subkey in subkeys) {
            if (string.IsNullOrEmpty(subkey)) {
              continue;
            }
            int index = int.Parse(subkey);
            yield return index;
          }
          continue;
        }
        if (key.IsMemberIdentifier()) {
          yield return key;
          continue;
        }
        throw new Exception($"invalid path: {fieldPath}");
      }
    }

    //======================================================================

    private static readonly Regex elementIdentifier = new Regex(@"^[_a-zA-Z][_a-zA-Z0-9]*(\[[0-9]*\])+$");
    // e.g. "foo[0][1]"

    private static readonly Regex elementIndex = new Regex(@"^(\[[0-9]*\])+$");

    private static readonly Regex memberIdentifier = new Regex(@"^[_a-zA-Z][_a-zA-Z0-9]*$");
    // e.g. "foo"

    //======================================================================

    private static bool IsElementIdentifier(this string s) {
      return elementIdentifier.IsMatch(s);
    }

    private static bool IsElementIndex(this string s) {
      return elementIndex.IsMatch(s);
    }

    private static bool IsMemberIdentifier(this string s) {
      return memberIdentifier.IsMatch(s);
    }

  }

}
#endif