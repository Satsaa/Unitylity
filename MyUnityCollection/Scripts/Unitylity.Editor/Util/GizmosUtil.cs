
namespace Unitylity.Editor {

	using System;
	using UnityEngine;

	public static class GizmosUtil {

		public static Deferred ColorScope() {
			var prev = Gizmos.color;
			return new Deferred(() => Gizmos.color = prev);
		}

		public static Deferred ColorScope(Color color) {
			var prev = Gizmos.color;
			Gizmos.color = color;
			return new Deferred(() => Gizmos.color = prev);
		}

		public static Deferred ColorScope(Func<Color, Color> modifier) {
			var prev = Gizmos.color;
			Gizmos.color = modifier(prev);
			return new Deferred(() => Gizmos.color = prev);
		}


		public static Deferred ExposureScope() {
			var prev = Gizmos.exposure;
			return new Deferred(() => Gizmos.exposure = prev);
		}

		public static Deferred ExposureScope(Texture texture) {
			var prev = Gizmos.exposure;
			Gizmos.exposure = texture;
			return new Deferred(() => Gizmos.exposure = prev);
		}

		public static Deferred ExposureScope(Func<Texture, Texture> modifier) {
			var prev = Gizmos.exposure;
			Gizmos.exposure = modifier(prev);
			return new Deferred(() => Gizmos.exposure = prev);
		}


		public static Deferred MatrixScope() {
			var prev = Gizmos.matrix;
			return new Deferred(() => Gizmos.matrix = prev);
		}

		public static Deferred MatrixScope(Matrix4x4 matrix) {
			var prev = Gizmos.matrix;
			Gizmos.matrix = matrix;
			return new Deferred(() => Gizmos.matrix = prev);
		}

		public static Deferred MatrixScope(Func<Matrix4x4, Matrix4x4> modifier) {
			var prev = Gizmos.matrix;
			Gizmos.matrix = modifier(prev);
			return new Deferred(() => Gizmos.matrix = prev);
		}

	}

}