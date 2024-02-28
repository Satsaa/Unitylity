
namespace Unitylity.Data {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Unitylity.Extensions;
	using UnityEngine;

	public class SerializedTypeComparer : IEqualityComparer<SerializedType> {
		public bool Equals(SerializedType x, SerializedType y) {
			if (x is null) return y is null;
			return x.type == y.type;
		}

		public int GetHashCode(SerializedType obj) {
			return obj?.type == null ? 0 : obj.type.GetHashCode();
		}
	}

	[Serializable]
	public class SerializedType : ISerializationCallbackReceiver {

		public static implicit operator Type(SerializedType t) => t?.type;

		[NonSerialized]
		protected bool updated = false;

		[SerializeField]
		protected string _name;
		public string name {
			get {
				return _name;
			}
			set {
				_name = value;
				Update();
			}
		}

		protected Type _type;
		public Type type {
			get {
				if (!updated) Update();
				return _type;
			}
			set {
				_type = value;
				_name = _type?.GetShortQualifiedName();
			}
		}

		protected virtual void Update() {
			if (_name == null) _type = null;
			else _type = Type.GetType(_name);
			updated = true;
		}

		public virtual IEnumerable<Type> GetValidTypes() {
			return AppDomain.CurrentDomain.GetAssemblies().SelectMany(v => v.GetTypes());
		}

		public virtual void OnBeforeSerialize() { }
		public virtual void OnAfterDeserialize() {
			if (!String.IsNullOrEmpty(_name)) {
				var nameWas = _name;
				Update();
				if (_type == null) {
					Debug.LogWarning($"Type for \"{nameWas}\" was not found.");
				}
			} else {
				Update();
			}
		}

		public bool Equals(SerializedType x, SerializedType y) {
			if (x is null) return y is null;
			return x.type == y.type;
		}

		public int GetHashCode(SerializedType obj) {
			return obj?.type == null ? 0 : obj.type.GetHashCode();
		}
	}

	[Serializable]
	public class SerializedType<T> : SerializedType {

		public static implicit operator Type(SerializedType<T> t) => t?.type;

		protected override void Update() {
			var newtype = Type.GetType(_name ?? "");
			if (newtype != null) _type = typeof(T).IsAssignableFrom(newtype) ? newtype : null;
			updated = true;
		}

		public override IEnumerable<Type> GetValidTypes() {
			return base.GetValidTypes().Where(v => v == typeof(T) || typeof(T).IsAssignableFrom(v));
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Data.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;

	[CanEditMultipleObjects]
	[CustomPropertyDrawer(typeof(SerializedType), true)]
	public class SerializedTypeDrawer : PropertyDrawer {

		protected virtual string GetDropdownText(SerializedType value) {
			return value == null
				? "ERROR"
				: value.type == null
					? String.IsNullOrWhiteSpace(value.name)
						? "None"
						: $"Missing ({value.name})"
					: $"{value.type} ({value.type.Assembly.GetName().Name})";
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			var noLabel = label.text is "" && label.image is null;

			var values = GetValues<SerializedType>(property);
			var value = values.First();

			using (PropertyScope(position, label, property, out label)) {
				// Label
				if (!noLabel) {
					EditorGUI.LabelField(position, label);
					position.xMin += EditorGUIUtility.labelWidth + spacing;
				}
				// Dropdown
				var hint = new GUIContent(label) { // Copy main label
					text = GetDropdownText(value)
				};
				if (EditorGUI.DropdownButton(position, new GUIContent(hint), FocusType.Keyboard)) {
					var types = value.GetValidTypes();
					var menu = TypeSelectMenu(types.ToList(), values.Select(v => v.type), type => OnSelect(property, type), true);
					menu.DropDown(position);
				}
			}
		}


		protected static void OnSelect(SerializedProperty property, Type type) {
			var values = GetValues<SerializedType>(property);
			Undo.RecordObjects(property.serializedObject.targetObjects, $"Set {property.name}");
			foreach (var value in values) value.type = type;
			foreach (var target in property.serializedObject.targetObjects) EditorUtility.SetDirty(target);
			property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
		}

	}

}
#endif