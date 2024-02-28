#if UNITY_EDITOR
namespace Unitylity.Editor {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Text.RegularExpressions;
	using UnityEditor;
	using UnityEngine;
	using Unitylity.Extensions;


	public static class PropertyUtil {

		/// <summary>
		/// Get the FieldInfo of the serialized property. Does not support lists or arrays.
		/// </summary>
		public static FieldInfo GetFieldInfo(SerializedProperty property) {
			string propertyPath = property.propertyPath;
			var match = endsWithArrayElementRegex.Match(propertyPath);
			if (match.Success) propertyPath = propertyPath.Remove(match.Index - 1);
			object value = property.serializedObject.targetObject;
			FieldInfo res = default;
			int cursor = 0;
			while (NextPathComponent(propertyPath, ref cursor, out var token)) {
				res = GetPathComponentFieldInfo(value, token);
				value = GetPathComponentValue(value, token);
			}
			return res;
		}

		/// <summary>
		/// Get the type of the serialized property, even if its a list or array.
		/// </summary>
		public static Type GetPropertyType(SerializedProperty property) {
			string propertyPath = property.propertyPath;
			var match = endsWithArrayElementRegex.Match(propertyPath);
			if (match.Success) propertyPath = propertyPath.Remove(match.Index - 1);
			object value = property.serializedObject.targetObject;
			FieldInfo fi = default;
			int cursor = 0;
			while (NextPathComponent(propertyPath, ref cursor, out var token)) {
				fi = GetPathComponentFieldInfo(value, token);
				value = GetPathComponentValue(value, token);
			}
			if (fi.FieldType.IsArray) {
				return fi.FieldType.GetElementType();
			} else {
				var generic = fi.FieldType.GetGenericTypeOf(typeof(List<>));
				if (generic != null) return generic;
			}
			return fi.FieldType;
		}

		/// <summary>
		/// Gets the first valid value of a SerializedProperty.
		/// </summary>
		public static T GetFirstValue<T>(SerializedProperty property) {
			string propertyPath = property.propertyPath;
			foreach (object targetObject in property.serializedObject.targetObjects) {
				var value = targetObject;
				int cursor = 0;
				while (NextPathComponent(propertyPath, ref cursor, out var token))
					value = GetPathComponentValue(value, token);
				if (value != null) return (T)value;
			}
			return default;
		}

		/// <summary>
		/// Gets all the values of a SerializedProperty.
		/// </summary>
		public static IEnumerable<T> GetValues<T>(SerializedProperty property) {
			string propertyPath = property.propertyPath;
			foreach (var targetObject in property.serializedObject.targetObjects) {
				object value = targetObject;
				int cursor = 0;
				while (NextPathComponent(propertyPath, ref cursor, out var token))
					value = GetPathComponentValue(value, token);
				yield return (T)value;
			}
		}

		/// <summary>
		/// Set the value of the serialized property in all target Objects.
		/// </summary>
		public static void SetValue(SerializedProperty property, object value) {
			var targets = property.serializedObject.targetObjects;
			foreach (var targetObject in targets) Undo.RecordObject(targetObject, $"Set {property.name}");
			SetValueNoRecord(property, value);
			foreach (var targetObject in targets) EditorUtility.SetDirty(targetObject);
			property.serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// Set the value of the serialized property, but do not record the change.
		/// The change will not be persisted unless you call SetDirty and ApplyModifiedProperties.
		/// </summary>
		public static void SetValueNoRecord(SerializedProperty property, object value) {
			string propertyPath = property.propertyPath;
			var targets = property.serializedObject.targetObjects;
			foreach (var target in targets) {
				object container = target;

				int cursor = 0;
				NextPathComponent(propertyPath, ref cursor, out var pathComponent);
				while (NextPathComponent(propertyPath, ref cursor, out var token)) {
					container = GetPathComponentValue(container, pathComponent);
					pathComponent = token;
				}
				Debug.Assert(!container.GetType().IsValueType, $"Cannot use SerializedObject.SetValue on a struct object, as the result will be set on a temporary.  Either change {container.GetType().Name} to a class, or use SetValue with a parent member.");
				SetPathComponentValue(container, pathComponent, value);
			}
		}

		// Union type representing either a property name or array element index.  The element
		// index is valid only if propertyName is null.
		readonly struct PropertyPathComponent {
			public readonly string propertyName;
			public readonly int elementIndex;
			public PropertyPathComponent(string propertyName) : this() {
				this.propertyName = propertyName;
			}

			public PropertyPathComponent(int elementIndex) : this() {
				this.elementIndex = elementIndex;
			}
		}

		static readonly Regex endsWithArrayElementRegex = new(@"Array\.data\[(\d+)\]$", RegexOptions.Compiled);
		static readonly Regex arrayElementRegex = new(@"\GArray\.data\[(\d+)\]", RegexOptions.Compiled);

		// Parse the next path component from a SerializedProperty.propertyPath.  For simple field/property access,
		// this is just tokenizing on '.' and returning each field/property name.  Array/list access is via
		// the pseudo-property "Array.data[N]", so this method parses that and returns just the array/list index N.
		//
		// Call this method repeatedly to access all path components.  For example:
		//
		//      string propertyPath = "quests.Array.data[0].goal";
		//      int i = 0;
		//      NextPropertyPathToken(propertyPath, ref i, out var component);
		//          => component = { propertyName = "quests" };
		//      NextPropertyPathToken(propertyPath, ref i, out var component) 
		//          => component = { elementIndex = 0 };
		//      NextPropertyPathToken(propertyPath, ref i, out var component) 
		//          => component = { propertyName = "goal" };
		//      NextPropertyPathToken(propertyPath, ref i, out var component) 
		//          => returns false
		static bool NextPathComponent(string propertyPath, ref int cursor, out PropertyPathComponent component) {

			if (cursor >= propertyPath.Length) {
				component = default;
				return false;
			}

			var arrayElementMatch = arrayElementRegex.Match(propertyPath, cursor);
			if (arrayElementMatch.Success) {
				cursor += arrayElementMatch.Length + 1; // Skip past next '.'
				component = new(int.Parse(arrayElementMatch.Groups[1].Value));
				return true;
			}

			int dot = propertyPath.IndexOf('.', cursor);
			if (dot == -1) {
				component = new(propertyPath[cursor..]);
				cursor = propertyPath.Length;
			} else {
				component = new(propertyPath[cursor..dot]);
				cursor = dot + 1; // Skip past next '.'
			}

			return true;
		}

		static object GetPathComponentValue(object container, PropertyPathComponent component) {
			if (component.propertyName == null)
				return ((IList)container)[component.elementIndex];
			else
				return GetMemberValue(container, component.propertyName);
		}

		static void SetPathComponentValue(object container, PropertyPathComponent component, object value) {
			if (component.propertyName == null)
				((IList)container)[component.elementIndex] = value;
			else
				SetMemberValue(container, component.propertyName, value);
		}

		static object GetMemberValue(object container, string name) {
			if (container == null)
				return null;
			var type = container.GetType();
			var field = GetField(type, name);
			if (field != null) {
				return field.GetValue(container);
			}
			return null;
		}

		static void SetMemberValue(object container, string name, object value) {
			var type = container.GetType();
			var field = GetField(type, name);
			if (field != null) {
				field.SetValue(container, value);
				return;
			}
			Debug.LogError($"Failed to set member {container}.{name} via reflection");
		}

		static FieldInfo GetPathComponentFieldInfo(object container, PropertyPathComponent component) {
			if (component.propertyName == null)
				return null;
			else
				return GetField(container.GetType(), component.propertyName);
		}

		static FieldInfo GetField(Type type, string name) {
			var current = type;
			while (current != null) {
				var field = current.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (field != null)
					return field;
				current = current.BaseType;
			}
			return null;
		}

	}

}
#endif