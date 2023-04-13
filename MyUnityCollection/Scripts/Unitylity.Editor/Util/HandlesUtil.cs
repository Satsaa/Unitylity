#if UNITY_EDITOR
namespace Unitylity.Editor {

	using System;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.Rendering;

	public static class HandlesUtil {

		public static Deferred ColorScope() {
			var prev = Handles.color;
			return new Deferred(() => Handles.color = prev);
		}

		public static Deferred ColorScope(Color color) {
			var prev = Handles.color;
			Handles.color = color;
			return new Deferred(() => Handles.color = prev);
		}

		public static Deferred ColorScope(Func<Color, Color> modifier) {
			var prev = Handles.color;
			Handles.color = modifier(prev);
			return new Deferred(() => Handles.color = prev);
		}


		public static Deferred LightinScope() {
			var prev = Handles.lighting;
			return new Deferred(() => Handles.lighting = prev);
		}

		public static Deferred LightinScope(bool lit) {
			var prev = Handles.lighting;
			Handles.lighting = lit;
			return new Deferred(() => Handles.lighting = prev);
		}

		public static Deferred LightinScope(Func<bool, bool> modifier) {
			var prev = Handles.lighting;
			Handles.lighting = modifier(prev);
			return new Deferred(() => Handles.lighting = prev);
		}


		public static Deferred MatrixScope() {
			var prev = Handles.matrix;
			return new Deferred(() => Handles.matrix = prev);
		}

		public static Deferred MatrixScope(Matrix4x4 matrix) {
			var prev = Handles.matrix;
			Handles.matrix = matrix;
			return new Deferred(() => Handles.matrix = prev);
		}

		public static Deferred MatrixScope(Func<Matrix4x4, Matrix4x4> modifier) {
			var prev = Handles.matrix;
			Handles.matrix = modifier(prev);
			return new Deferred(() => Handles.matrix = prev);
		}


		public static Deferred ZTestScope() {
			var prev = Handles.zTest;
			return new Deferred(() => Handles.zTest = prev);
		}

		public static Deferred ZTestScope(CompareFunction comparer) {
			var prev = Handles.zTest;
			Handles.zTest = comparer;
			return new Deferred(() => Handles.zTest = prev);
		}

		public static Deferred ZTestScope(Func<CompareFunction, CompareFunction> modifier) {
			var prev = Handles.zTest;
			Handles.zTest = modifier(prev);
			return new Deferred(() => Handles.zTest = prev);
		}

	}

}
#endif