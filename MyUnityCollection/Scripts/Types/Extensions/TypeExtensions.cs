
namespace Muc.Types.Extensions {

  using System;
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public static class TypeExtensions {

    public static bool IsGenericTypeOf(this Type type, Type generic) {
      while (type != null && type != typeof(object)) {
        var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        if (generic == cur) return true;
        type = type.BaseType;
      }
      return false;
    }
  }

}