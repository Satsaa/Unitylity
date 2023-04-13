
namespace Unitylity.Geometry {

	using System;
	using UnityEngine;

	[Serializable]
	public struct Line {

		public Vector3 start;
		public Vector3 end;

		public Vector3 dir => dif.normalized;
		public Vector3 dif => end - start;
		public float magnitude => length;
		public float length => dif.magnitude;
		public float lengthsq => dif.sqrMagnitude;
		public Line reversed => new(end, start);

		public Line(Vector3 start, Vector3 end) {
			this.start = start;
			this.end = end;
		}

		public void Debug() => UnityEngine.Debug.DrawLine(start, end);
		public void Debug(Color color, float duration = 0) => UnityEngine.Debug.DrawLine(start, end, color, duration);

		public void Reverse() {
			var temp = start;
			start = end;
			end = temp;
		}

		public Vector3 ClampPoint(Vector3 point) {

			if (dif == Vector3.zero) return start;

			// get vector from point on infinite line to point in space
			var lineVec = dif.normalized;
			Vector3 linePointToPoint = point - start;
			float t = Vector3.Dot(linePointToPoint, lineVec);
			Vector3 projectedPoint = start + lineVec * t;

			Side side = PointSide(projectedPoint);

			// The projected point is on the line segment
			return side switch {
				Side.Inside => projectedPoint,
				Side.Start => start,
				Side.End => end,
				_ => throw new Exception("No valid point was found on the line. This should never happen on valid Lines"),
			};
		}

		public enum Side { Inside, Start, End, }
		public Side PointSide(Vector3 point) {
			Vector3 dir = end - start;
			Vector3 pointVec = point - start;
			float dot = Vector3.Dot(pointVec, dir);
			// point is on side of linePoint2, compared to linePoint1
			if (dot > 0) {
				// point is on the line segment
				if (pointVec.sqrMagnitude <= lengthsq) {
					return Side.Inside;
				}
				// point is not on the line segment and it is on the side of linePoint2
				else {
					return Side.End;
				}
			}
			// Point is not on side of linePoint2, compared to linePoint1.
			// Point is not on the line segment and it is on the side of linePoint1.
			else {
				return Side.Start;
			}
		}

		/// <summary> Returns the shortest Line connecting `this` and `line` </summary>
		public Line ShortestConnectingLine(Line line) => ShortestConnectingLine(this, line);

		/// <summary> Returns the shortest Line connecting `line1` and `line2` </summary>
		public static Line ShortestConnectingLine(Line line1, Line line2) {

			ShortestLineConnectingTwoInfiniteLines(out var res, line1.start, line1.dir, line2.start, line2.dir);

			var startSide = line1.PointSide(res.start);
			if (startSide == Side.Start) res.start = line1.start;
			else if (startSide == Side.End) res.start = line1.end;

			var endSide = line2.PointSide(res.end);
			if (endSide == Side.Start) res.end = line2.start;
			else if (endSide == Side.End) res.end = line2.end;

			if (endSide != Side.Inside)
				res.start = line1.ClampPoint(res.end);
			if (startSide != Side.Inside)
				res.end = line2.ClampPoint(res.start);

			return res;
		}

		/// <summary> Returns true if the two lines are not parallel, which allows creating a definite output line. Outputs the shortest Line connecting two infinite lines.  </summary>
		public static bool ShortestLineConnectingTwoInfiniteLines(out Line shortestLine, Vector3 linePoint1, Vector3 lineDir1, Vector3 linePoint2, Vector3 lineDir2) {

			shortestLine = new Line(Vector3.zero, Vector3.zero);

			float a = Vector3.Dot(lineDir1, lineDir1);
			float b = Vector3.Dot(lineDir1, lineDir2);
			float e = Vector3.Dot(lineDir2, lineDir2);

			float d = a * e - b * b;

			// lines are not parallel
			if (d != 0.0f) {

				Vector3 r = linePoint1 - linePoint2;
				float c = Vector3.Dot(lineDir1, r);
				float f = Vector3.Dot(lineDir2, r);

				float s = (b * f - c * e) / d;
				float t = (a * f - c * b) / d;

				shortestLine.start = linePoint1 + lineDir1 * s;
				shortestLine.end = linePoint2 + lineDir2 * t;

				return true;
			} else {
				return false;
			}
		}

	}

}