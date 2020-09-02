

namespace Muc.Extensions {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public static class TypeExtensions {

    public static bool IsGenericTypeOf(this Type type, Type genericType) {
      while (type != null && type != typeof(object)) {
        var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        if (genericType == cur) return true;
        type = type.BaseType;
      }
      return false;
    }

    public static IEnumerable<Type> BaseTypes(this Type type) {
      while (type != null && type != typeof(object)) {
        type = type.BaseType;
        yield return type;
      }
    }
  }

}