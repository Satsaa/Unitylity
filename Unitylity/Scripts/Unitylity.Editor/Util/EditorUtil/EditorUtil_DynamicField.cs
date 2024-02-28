#if UNITY_EDITOR
namespace Unitylity.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;
	using Unitylity.Extensions;
	using Unitylity.Numerics;
	using Object = UnityEngine.Object;

	public static partial class EditorUtil {

		/// <summary> DynamicField selects the best fitting field to draw the field </summary>
		public static object DynamicField(Rect position, object value) {
			switch (value) {
				case Object objectVal:
					return Field(position, objectVal);
				case string stringVal:
					return Field(position, stringVal);
				case int intVal:
					return Field(position, intVal);
				case long longVal:
					return Field(position, longVal);
				case bool boolVal:
					return Field(position, boolVal);
				case float floatVal:
					return Field(position, floatVal);
				case double doubleVal:
					return Field(position, doubleVal);
				case Rect rectVal:
					return Field(position, rectVal);
				case RectInt rectIntVal:
					return Field(position, rectIntVal);
				case Bounds boundVal:
					return Field(position, boundVal);
				case Color colorVal:
					return Field(position, colorVal);
				case AnimationCurve animationCurveVal:
					return Field(position, animationCurveVal);
				case Gradient gradientVal:
					return Field(position, gradientVal);
				case Vector2 vector2Val:
					return Field(position, vector2Val);
				case Vector2Int vector2IntVal:
					return Field(position, vector2IntVal);
				case Vector3 vector3Val:
					return Field(position, vector3Val);
				case Vector3Int vector3IntVal:
					return Field(position, vector3IntVal);
				case Vector4 vector4Val:
					return Field(position, vector4Val);
				case Enum enumVal:
					if (enumVal.GetType().IsDefined(typeof(FlagsAttribute), false)) {
						return EditorGUI.EnumFlagsField(position, enumVal);
					} else {
						return EditorGUI.EnumPopup(position, enumVal);
					}
			}
			return value;
		}
		/// <summary> DynamicField selects the best fitting field to draw the field </summary>
		public static object DynamicField(object value) {
			switch (value) {
				case Object objectVal:
					return Field(objectVal);
				case string stringVal:
					return Field(stringVal);
				case int intVal:
					return Field(intVal);
				case long longVal:
					return Field(longVal);
				case bool boolVal:
					return Field(boolVal);
				case float floatVal:
					return Field(floatVal);
				case double doubleVal:
					return Field(doubleVal);
				case Rect rectVal:
					return Field(rectVal);
				case RectInt rectIntVal:
					return Field(rectIntVal);
				case Bounds boundVal:
					return Field(boundVal);
				case Color colorVal:
					return Field(colorVal);
				case AnimationCurve animationCurveVal:
					return Field(animationCurveVal);
				case Gradient gradientVal:
					return Field(gradientVal);
				case Vector2 vector2Val:
					return Field(vector2Val);
				case Vector2Int vector2IntVal:
					return Field(vector2IntVal);
				case Vector3 vector3Val:
					return Field(vector3Val);
				case Vector3Int vector3IntVal:
					return Field(vector3IntVal);
				case Vector4 vector4Val:
					return Field(vector4Val);
				case Enum enumVal:
					if (enumVal.GetType().IsDefined(typeof(FlagsAttribute), false)) {
						return EditorGUILayout.EnumFlagsField(enumVal);
					} else {
						return EditorGUILayout.EnumPopup(enumVal);
					}
			}
			return value;
		}
		/// <summary> DynamicField selects the best fitting field to draw the field </summary>
		public static object DynamicField(Rect position, GUIContent label, object value) {
			switch (value) {
				case Object objectVal:
					return Field(position, label, objectVal);
				case string stringVal:
					return Field(position, label, stringVal);
				case int intVal:
					return Field(position, label, intVal);
				case long longVal:
					return Field(position, label, longVal);
				case bool boolVal:
					return Field(position, label, boolVal);
				case float floatVal:
					return Field(position, label, floatVal);
				case double doubleVal:
					return Field(position, label, doubleVal);
				case Rect rectVal:
					return Field(position, label, rectVal);
				case RectInt rectIntVal:
					return Field(position, label, rectIntVal);
				case Bounds boundVal:
					return Field(position, label, boundVal);
				case Color colorVal:
					return Field(position, label, colorVal);
				case AnimationCurve animationCurveVal:
					return Field(position, label, animationCurveVal);
				case Gradient gradientVal:
					return Field(position, label, gradientVal);
				case Vector2 vector2Val:
					return Field(position, label, vector2Val);
				case Vector2Int vector2IntVal:
					return Field(position, label, vector2IntVal);
				case Vector3 vector3Val:
					return Field(position, label, vector3Val);
				case Vector3Int vector3IntVal:
					return Field(position, label, vector3IntVal);
				case Vector4 vector4Val:
					return Field(position, label, vector4Val);
				case Enum enumVal:
					if (enumVal.GetType().IsDefined(typeof(FlagsAttribute), false)) {
						return EditorGUI.EnumFlagsField(position, label, enumVal);
					} else {
						return EditorGUI.EnumPopup(position, label, enumVal);
					}
			}
			return value;
		}
		/// <summary> DynamicField selects the best fitting field to draw the field </summary>
		public static object DynamicField(GUIContent label, object value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				switch (value) {
					case Object objectVal:
						return Field(indented, label, objectVal);
					case string stringVal:
						return Field(indented, label, stringVal);
					case int intVal:
						return Field(indented, label, intVal);
					case long longVal:
						return Field(indented, label, longVal);
					case bool boolVal:
						return Field(indented, label, boolVal);
					case float floatVal:
						return Field(indented, label, floatVal);
					case double doubleVal:
						return Field(indented, label, doubleVal);
					case Rect rectVal:
						return Field(indented, label, rectVal);
					case RectInt rectIntVal:
						return Field(indented, label, rectIntVal);
					case Bounds boundVal:
						return Field(indented, label, boundVal);
					case Color colorVal:
						return Field(indented, label, colorVal);
					case AnimationCurve animationCurveVal:
						return Field(indented, label, animationCurveVal);
					case Gradient gradientVal:
						return Field(indented, label, gradientVal);
					case Vector2 vector2Val:
						return Field(indented, label, vector2Val);
					case Vector2Int vector2IntVal:
						return Field(indented, label, vector2IntVal);
					case Vector3 vector3Val:
						return Field(indented, label, vector3Val);
					case Vector3Int vector3IntVal:
						return Field(indented, label, vector3IntVal);
					case Vector4 vector4Val:
						return Field(indented, label, vector4Val);
					case Enum enumVal:
						if (enumVal.GetType().IsDefined(typeof(FlagsAttribute), false)) {
							return EditorGUI.EnumFlagsField(indented, label, enumVal);
						} else {
							return EditorGUI.EnumPopup(indented, label, enumVal);
						}
				}
				return value;
			}
		}



		/// <summary> TypedField selects the best fitting field to draw the field based on the given type </summary>
		public static object TypedField(Rect position, object value, Type type, object defaultValue = null) {
			if (type == typeof(Object)) {
				return Field(position, (Object)(value ?? defaultValue ?? default(Object)));
			} else if (type == typeof(string))
				return Field(position, (string)(value ?? defaultValue ?? default(string)));
			else if (type == typeof(int))
				return Field(position, (int)(value ?? defaultValue ?? default(int)));
			else if (type == typeof(long))
				return Field(position, (long)(value ?? defaultValue ?? default(long)));
			else if (type == typeof(bool))
				return Field(position, (bool)(value ?? defaultValue ?? default(bool)));
			else if (type == typeof(float))
				return Field(position, (float)(value ?? defaultValue ?? default(float)));
			else if (type == typeof(double))
				return Field(position, (double)(value ?? defaultValue ?? default(double)));
			else if (type == typeof(Rect))
				return Field(position, (Rect)(value ?? defaultValue ?? default(Rect)));
			else if (type == typeof(RectInt))
				return Field(position, (RectInt)(value ?? defaultValue ?? default(RectInt)));
			else if (type == typeof(Bounds))
				return Field(position, (Bounds)(value ?? defaultValue ?? default(Bounds)));
			else if (type == typeof(Color))
				return Field(position, (Color)(value ?? defaultValue ?? default(Color)));
			else if (type == typeof(AnimationCurve))
				return Field(position, (AnimationCurve)(value ?? defaultValue ?? default(AnimationCurve)));
			else if (type == typeof(Gradient))
				return Field(position, (Gradient)(value ?? defaultValue ?? default(Gradient)));
			else if (type == typeof(Vector2))
				return Field(position, (Vector2)(value ?? defaultValue ?? default(Vector2)));
			else if (type == typeof(Vector2Int))
				return Field(position, (Vector2Int)(value ?? defaultValue ?? default(Vector2Int)));
			else if (type == typeof(Vector3))
				return Field(position, (Vector3)(value ?? defaultValue ?? default(Vector3)));
			else if (type == typeof(Vector3Int))
				return Field(position, (Vector3Int)(value ?? defaultValue ?? default(Vector3Int)));
			else if (type == typeof(Vector4))
				return Field(position, (Vector4)(value ?? defaultValue ?? default(Vector4)));
			else if (type == typeof(Enum)) {
				var enumVal = (Enum)value;
				if (enumVal.GetType().IsDefined(typeof(FlagsAttribute), false)) {
					return EditorGUI.EnumFlagsField(position, enumVal);
				} else {
					return EditorGUI.EnumPopup(position, enumVal);
				}
			}
			return value;
		}
		/// <summary> TypedField selects the best fitting field to draw the field based on the given type </summary>
		public static object TypedField(object value, Type type, object defaultValue = null) {
			if (type == typeof(Object))
				return Field((Object)(value ?? defaultValue ?? default(Object)));
			else if (type == typeof(string))
				return Field((string)(value ?? defaultValue ?? default(string)));
			else if (type == typeof(int))
				return Field((int)(value ?? defaultValue ?? default(int)));
			else if (type == typeof(long))
				return Field((long)(value ?? defaultValue ?? default(long)));
			else if (type == typeof(bool))
				return Field((bool)(value ?? defaultValue ?? default(bool)));
			else if (type == typeof(float))
				return Field((float)(value ?? defaultValue ?? default(float)));
			else if (type == typeof(double))
				return Field((double)(value ?? defaultValue ?? default(double)));
			else if (type == typeof(Rect))
				return Field((Rect)(value ?? defaultValue ?? default(Rect)));
			else if (type == typeof(RectInt))
				return Field((RectInt)(value ?? defaultValue ?? default(RectInt)));
			else if (type == typeof(Bounds))
				return Field((Bounds)(value ?? defaultValue ?? default(Bounds)));
			else if (type == typeof(Color))
				return Field((Color)(value ?? defaultValue ?? default(Color)));
			else if (type == typeof(AnimationCurve))
				return Field((AnimationCurve)(value ?? defaultValue ?? default(AnimationCurve)));
			else if (type == typeof(Gradient))
				return Field((Gradient)(value ?? defaultValue ?? default(Gradient)));
			else if (type == typeof(Vector2))
				return Field((Vector2)(value ?? defaultValue ?? default(Vector2)));
			else if (type == typeof(Vector2Int))
				return Field((Vector2Int)(value ?? defaultValue ?? default(Vector2Int)));
			else if (type == typeof(Vector3))
				return Field((Vector3)(value ?? defaultValue ?? default(Vector3)));
			else if (type == typeof(Vector3Int))
				return Field((Vector3Int)(value ?? defaultValue ?? default(Vector3Int)));
			else if (type == typeof(Vector4))
				return Field((Vector4)(value ?? defaultValue ?? default(Vector4)));
			else if (type == typeof(Enum)) {
				var enumVal = (Enum)value;
				if (enumVal.GetType().IsDefined(typeof(FlagsAttribute), false)) {
					return EditorGUILayout.EnumFlagsField(enumVal);
				} else {
					return EditorGUILayout.EnumPopup(enumVal);
				}
			}
			return value;
		}
		/// <summary> TypedField selects the best fitting field to draw the field based on the given type </summary>
		public static object TypedField(Rect position, GUIContent label, object value, Type type, object defaultValue = null) {
			if (type == typeof(Object))
				return Field(position, label, (Object)(value ?? defaultValue ?? default(Object)));
			else if (type == typeof(string))
				return Field(position, label, (string)(value ?? defaultValue ?? default(string)));
			else if (type == typeof(int))
				return Field(position, label, (int)(value ?? defaultValue ?? default(int)));
			else if (type == typeof(long))
				return Field(position, label, (long)(value ?? defaultValue ?? default(long)));
			else if (type == typeof(bool))
				return Field(position, label, (bool)(value ?? defaultValue ?? default(bool)));
			else if (type == typeof(float))
				return Field(position, label, (float)(value ?? defaultValue ?? default(float)));
			else if (type == typeof(double))
				return Field(position, label, (double)(value ?? defaultValue ?? default(double)));
			else if (type == typeof(Rect))
				return Field(position, label, (Rect)(value ?? defaultValue ?? default(Rect)));
			else if (type == typeof(RectInt))
				return Field(position, label, (RectInt)(value ?? defaultValue ?? default(RectInt)));
			else if (type == typeof(Bounds))
				return Field(position, label, (Bounds)(value ?? defaultValue ?? default(Bounds)));
			else if (type == typeof(Color))
				return Field(position, label, (Color)(value ?? defaultValue ?? default(Color)));
			else if (type == typeof(AnimationCurve))
				return Field(position, label, (AnimationCurve)(value ?? defaultValue ?? default(AnimationCurve)));
			else if (type == typeof(Gradient))
				return Field(position, label, (Gradient)(value ?? defaultValue ?? default(Gradient)));
			else if (type == typeof(Vector2))
				return Field(position, label, (Vector2)(value ?? defaultValue ?? default(Vector2)));
			else if (type == typeof(Vector2Int))
				return Field(position, label, (Vector2Int)(value ?? defaultValue ?? default(Vector2Int)));
			else if (type == typeof(Vector3))
				return Field(position, label, (Vector3)(value ?? defaultValue ?? default(Vector3)));
			else if (type == typeof(Vector3Int))
				return Field(position, label, (Vector3Int)(value ?? defaultValue ?? default(Vector3Int)));
			else if (type == typeof(Vector4))
				return Field(position, label, (Vector4)(value ?? defaultValue ?? default(Vector4)));
			else if (type == typeof(Enum)) {
				var enumVal = (Enum)value;
				if (enumVal.GetType().IsDefined(typeof(FlagsAttribute), false)) {
					return EditorGUI.EnumFlagsField(position, label, enumVal);
				} else {
					return EditorGUI.EnumPopup(position, label, enumVal);
				}
			}
			return value;
		}
		/// <summary> TypedField selects the best fitting field to draw the field based on the given type </summary>
		public static object TypedField(GUIContent label, object value, Type type, object defaultValue = null) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				if (type == typeof(Object))
					return Field(indented, label, (Object)(value ?? defaultValue ?? default(Object)));
				else if (type == typeof(string))
					return Field(indented, label, (string)(value ?? defaultValue ?? default(string)));
				else if (type == typeof(int))
					return Field(indented, label, (int)(value ?? defaultValue ?? default(int)));
				else if (type == typeof(long))
					return Field(indented, label, (long)(value ?? defaultValue ?? default(long)));
				else if (type == typeof(bool))
					return Field(indented, label, (bool)(value ?? defaultValue ?? default(bool)));
				else if (type == typeof(float))
					return Field(indented, label, (float)(value ?? defaultValue ?? default(float)));
				else if (type == typeof(double))
					return Field(indented, label, (double)(value ?? defaultValue ?? default(double)));
				else if (type == typeof(Rect))
					return Field(indented, label, (Rect)(value ?? defaultValue ?? default(Rect)));
				else if (type == typeof(RectInt))
					return Field(indented, label, (RectInt)(value ?? defaultValue ?? default(RectInt)));
				else if (type == typeof(Bounds))
					return Field(indented, label, (Bounds)(value ?? defaultValue ?? default(Bounds)));
				else if (type == typeof(Color))
					return Field(indented, label, (Color)(value ?? defaultValue ?? default(Color)));
				else if (type == typeof(AnimationCurve))
					return Field(indented, label, (AnimationCurve)(value ?? defaultValue ?? default(AnimationCurve)));
				else if (type == typeof(Gradient))
					return Field(indented, label, (Gradient)(value ?? defaultValue ?? default(Gradient)));
				else if (type == typeof(Vector2))
					return Field(indented, label, (Vector2)(value ?? defaultValue ?? default(Vector2)));
				else if (type == typeof(Vector2Int))
					return Field(indented, label, (Vector2Int)(value ?? defaultValue ?? default(Vector2Int)));
				else if (type == typeof(Vector3))
					return Field(indented, label, (Vector3)(value ?? defaultValue ?? default(Vector3)));
				else if (type == typeof(Vector3Int))
					return Field(indented, label, (Vector3Int)(value ?? defaultValue ?? default(Vector3Int)));
				else if (type == typeof(Vector4))
					return Field(indented, label, (Vector4)(value ?? defaultValue ?? default(Vector4)));
				else if (type == typeof(Enum)) {
					var enumVal = (Enum)value;
					if (enumVal.GetType().IsDefined(typeof(FlagsAttribute), false)) {
						return EditorGUI.EnumFlagsField(indented, label, enumVal);
					} else {
						return EditorGUI.EnumPopup(indented, label, enumVal);
					}
				}
			}
			return value;
		}
	}

}
#endif