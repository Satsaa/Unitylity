
namespace Unitylity.Extensions {

	using UnityEngine;

	public static class RectExtensions {

		public static Vector2 ClampPoint(this Rect rect, Vector2 point) {
			if (point.x < rect.xMin) {
				if (point.y < rect.yMin) {
					return new Vector2(rect.xMin, rect.yMin);
				} else if (point.y > rect.yMax) {
					return new Vector2(rect.xMin, rect.yMax);
				} else {
					return new Vector2(point.x + rect.xMin - point.x, point.y);
				}
			} else if (point.x > rect.xMax) {
				if (point.y < rect.yMin) {
					return new Vector2(rect.xMax, rect.yMin);
				} else if (point.y > rect.yMax) {
					return new Vector2(rect.xMax, rect.yMax);
				} else {
					return new Vector2(point.x + point.x - rect.xMax, point.y);
				}
			} else {
				if (point.y < rect.yMin) {
					return new Vector2(point.x, point.y + rect.yMin - point.y);
				} else if (point.y > rect.yMax) {
					return new Vector2(point.x, point.y + point.y - rect.yMax);
				} else {
					return point;
				}
			}
		}

		public static Rect Scale(this Rect rect, Vector2 scale) {
			var res = rect;

			res.size *= scale;
			res.center = rect.center;

			return res;
		}

	}

}