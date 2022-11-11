
namespace Muc.Data {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using Object = UnityEngine.Object;

	/// <summary>
	/// Unsurprisingly this is literally a wrapper for a value of any type.
	/// Note that the value is only serialized if it supports it.
	/// </summary>
	[Serializable]
	public class ValueWrapper<T> {

		public T value;

		public ValueWrapper() { }
		public ValueWrapper(T value) => this.value = value;

	}


#if UNITY_EDITOR
	namespace Editor {

		using System;
		using System.Collections.Generic;
		using System.Linq;
		using UnityEditor;
		using UnityEngine;
		using static Muc.Editor.EditorUtil;
		using static Muc.Editor.PropertyUtil;
		using Object = UnityEngine.Object;

		[CanEditMultipleObjects]
		[CustomPropertyDrawer(typeof(ValueWrapper<>), true)]
		public class ValueWrapperDrawer : PropertyDrawer {

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
				var value = property.FindPropertyRelative("value");
				using (PropertyScope(position, label, property, out label)) {
					PropertyField(position, label, value);
				}
			}

		}

	}
#endif
}
