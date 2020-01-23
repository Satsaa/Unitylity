using UnityEngine;

public static class IntExtensions {
  public static int RoundToNearest(this int integer, int nearest) => Mathf.RoundToInt(integer / nearest) * nearest;

  public static int Remap(this int value, int from1, int to1, int from2, int to2) => (value - from1) / (to1 - from1) * (to2 - from2) + from2;
}
