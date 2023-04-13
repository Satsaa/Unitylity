
namespace Unitylity.Geometry {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	[Serializable]
	public class Spline {

		public float tension = 0f;
		public float alpha = 0.5f;
		public List<Vector3> controls;

		public Spline(IEnumerable<Vector3> controls, float alpha = 0.5f, float tension = 0f) {
			this.alpha = alpha;
			this.tension = tension;
			this.controls = controls.ToList();
		}

		public IEnumerable<Vector3> RenderSpline(int tesselation) {

			var first = 2 * controls[0] - controls[1];
			var last = 2 * controls.Last() - controls[^2];

			for (float t = 0; t <= controls.Count - 1; t += 1f / tesselation) {
				var p = Eval(t);
				yield return p;
			}
		}

		public Vector3 Eval(float t) {

			if (t <= 0) return controls.First();
			if (t >= controls.Count - 1) return controls.Last();

			var first = 2 * controls[0] - controls[1];
			var last = 2 * controls.Last() - controls[^2];

			var i = Mathf.FloorToInt(t);

			var p0 = i - 1 < 0 ? first : controls[i - 1];
			var p1 = controls[i];
			var p2 = controls[i + 1];
			var p3 = i + 2 >= controls.Count ? last : controls[i + 2];

			t %= 1f;
			return Eval(p0, p1, p2, p3, t, alpha, tension);

		}

		public static Vector3 Eval(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, float alpha, float tension) {
			var t0 = 0;
			var t1 = t0 + Mathf.Pow(Vector3.Distance(p0, p1), alpha);
			var t2 = t1 + Mathf.Pow(Vector3.Distance(p1, p2), alpha);
			var t3 = t2 + Mathf.Pow(Vector3.Distance(p2, p3), alpha);

			var m1 = (1 - tension) * (t2 - t1) * ((p0 - p1) / (t0 - t1) - (p0 - p2) / (t0 - t2) + (p1 - p2) / (t1 - t2));
			var m2 = (1 - tension) * (t2 - t1) * ((p1 - p2) / (t1 - t2) - (p1 - p3) / (t1 - t3) + (p2 - p3) / (t2 - t3));

			var a = 2 * p1 - 2 * p2 + m1 + m2;
			var b = -3 * p1 + 3 * p2 - 2 * m1 - m2;

			var p = a * t * t * t + b * t * t + m1 * t + p1;
			return p;

		}

		public static IEnumerable<Vector3> EvalPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int tesselation, float alpha, float tension) {
			var t0 = 0;
			var t1 = t0 + Mathf.Pow(Vector3.Distance(p0, p1), alpha);
			var t2 = t1 + Mathf.Pow(Vector3.Distance(p1, p2), alpha);
			var t3 = t2 + Mathf.Pow(Vector3.Distance(p2, p3), alpha);

			var m1 = (1 - tension) * (t2 - t1) * ((p0 - p1) / (t0 - t1) - (p0 - p2) / (t0 - t2) + (p1 - p2) / (t1 - t2));
			var m2 = (1 - tension) * (t2 - t1) * ((p1 - p2) / (t1 - t2) - (p1 - p3) / (t1 - t3) + (p2 - p3) / (t2 - t3));

			var a = 2 * p1 - 2 * p2 + m1 + m2;
			var b = -3 * p1 + 3 * p2 - 2 * m1 - m2;

			for (var j = 1f; j <= tesselation; j++) {
				var t = j / tesselation;
				var p = a * t * t * t + b * t * t + m1 * t + p1;
				yield return p;
			}
		}

	}

}