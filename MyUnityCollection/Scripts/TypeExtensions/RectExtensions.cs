using UnityEngine;

public static class RectExtensions {
  public static Vector2 ClosestPoint(this Rect rect, Vector2 point) {
    if (point.x < rect.xMin) {
      if (point.y < rect.yMin) {
        return new Vector2(rect.xMin, rect.yMin);
      } else if (point.y > rect.yMax) {
        return new Vector2(rect.xMin, rect.yMax);
      } else {
        return point.AddX(rect.xMin - point.x);
      }
    } else if (point.x > rect.xMax) {
      if (point.y < rect.yMin) {
        return new Vector2(rect.xMax, rect.yMin);
      } else if (point.y > rect.yMax) {
        return new Vector2(rect.xMax, rect.yMax);
      } else {
        return point.AddX(point.x - rect.xMax);
      }
    } else {
      if (point.y < rect.yMin) {
        return point.AddY(rect.yMin - point.y);
      } else if (point.y > rect.yMax) {
        return point.AddY(point.y - rect.yMax);
      } else {
        return point;
      }
    }
  }
}