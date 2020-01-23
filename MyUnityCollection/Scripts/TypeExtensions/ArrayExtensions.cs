using System.Collections;
using System.Collections.Generic;

static class ArrayExtensions {
  /// <summary> Reverses the array or part of it in place </summary>
  public static void Reverse<T>(this T[] array, int index, int length) => System.Array.Reverse(array, index, length);
  public static void Reverse<T>(this T[] array) => System.Array.Reverse(array);

  public delegate R MapFunc1<R, T>(T current);
  public delegate R MapFunc2<R, T>(T current, int index);
  public delegate R MapFunc3<R, T>(T current, int index, T[] array);

  /// <summary> Returns the resulting array if `Func` is ran on each element </summary>
  public static R[] Map<T, R>(this T[] array, MapFunc1<R, T> func) {
    R[] res = new R[array.Length];
    for (int i = 0; i < array.Length; i++) {
      res[i] = func(array[i]);
    }
    return res;
  }
  /// <summary> Returns the resulting array if `Func` is ran on each element </summary>
  public static R[] Map<T, R>(this T[] array, MapFunc2<R, T> func) {
    R[] res = new R[array.Length];
    for (int i = 0; i < array.Length; i++) {
      res[i] = func(array[i], i);
    }
    return res;
  }
  /// <summary> Returns the resulting array if `Func` is ran on each element </summary>
  public static R[] Map<T, R>(this T[] array, MapFunc3<R, T> func) {
    R[] res = new R[array.Length];
    for (int i = 0; i < array.Length; i++) {
      res[i] = func(array[i], i, array);
    }
    return res;
  }



  /// <summary> Returns a new array which is this array and one or more arrays merged together </summary>
  public static T[] Concat<T>(this T[] array, params T[][] arrays) {
    var len = array.Length;
    for (int i = 0; i < arrays.Length; i++)
      len += arrays[i].Length;

    var res = new T[array.Length + len];

    array.CopyTo(res, 0);
    var startI = array.Length;
    for (int i = 0; i < arrays.Length; i++) {
      arrays[i].CopyTo(res, startI);
      startI += arrays[i].Length;
    }
    return res as T[];
  }
}
