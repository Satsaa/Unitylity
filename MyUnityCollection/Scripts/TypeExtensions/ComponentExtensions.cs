using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtensions {
  public static T CopyComponent<T>(this T original, GameObject destination) where T : Component {
    System.Type type = original.GetType();
    T copy = destination.AddComponent(type) as T;
    // Copied fields can be restricted with BindingFlags
    System.Reflection.FieldInfo[] fields = type.GetFields();
    foreach (System.Reflection.FieldInfo field in fields) {
      field.SetValue(copy, field.GetValue(original));
    }
    return copy;
  }
}