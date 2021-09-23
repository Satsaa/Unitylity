
namespace Muc.Data {

	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using System.Linq;
	using System.Reflection;

	[Serializable]
	public abstract class SerializedMemberInfo : SerializedType {

		protected const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

		[SerializeField]
		protected string _memberName;
		public string memberName {
			get {
				return _memberName;
			}
			set {
				_memberName = value;
				Update();
			}
		}

		public override IEnumerable<Type> GetValidTypes() {
			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(v => v.GetTypes())
				.Where(v => v.IsClass || (v.IsValueType && !v.IsValueType));
		}

#if UNITY_EDITOR
		internal abstract MemberInfo _GetMemberInfo();
		internal abstract IEnumerable<MemberInfo> _GetValidMembers();
#endif
	}

	[Serializable]
	public abstract class SerializedMemberInfo<T> : SerializedMemberInfo where T : MemberInfo {

		protected T _memberInfo;
		public T memberInfo {
			get {
				if (!updated) Update();
				return _memberInfo;
			}
			set {
				_memberInfo = value;
				_memberName = _memberInfo?.Name;
			}
		}

		public override IEnumerable<Type> GetValidTypes() {
			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(v => v.GetTypes())
				.Where(v => v.IsClass || (v.IsValueType && !v.IsValueType));
		}

		public abstract IEnumerable<T> GetValidMembers();

#if UNITY_EDITOR
		internal override sealed MemberInfo _GetMemberInfo() => memberInfo;
		internal override sealed IEnumerable<MemberInfo> _GetValidMembers() => GetValidMembers();
#endif
	}

}

#if UNITY_EDITOR
namespace Muc.Data.Editor {

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using static Muc.Editor.PropertyUtil;
	using static Muc.Editor.EditorUtil;
	using System.Reflection;

	public abstract class SerializedMemberInfoDrawer<T> : SerializedTypeDrawer where T : SerializedMemberInfo {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			var noLabel = label.text is "" && label.image is null;

			var values = GetValues<T>(property);
			var value = values.First();

			using (PropertyScope(position, label, property, out label)) {
				// Label
				if (!noLabel) {
					EditorGUI.LabelField(position, label);
					position.xMin += EditorGUIUtility.labelWidth + spacing;
				}
				// Dropdown
				var hint = new GUIContent(label) { // Copy main label
					text =
						value == null
							? "ERROR"
							: value.type == null
								? String.IsNullOrEmpty(value.name)
									? "None"
									: $"Missing ({value.name})"
								: ($"{value.type}." + (
									String.IsNullOrEmpty(value.memberName)
										? "<None>"
										: value._GetMemberInfo() == null
											? $"{value.memberName} (Missing)"
											: value.memberName
								) + $" ({value.type.Assembly.GetName().Name})")
				};
				if (value.type == null) {
					if (EditorGUI.DropdownButton(position, new GUIContent(hint), FocusType.Keyboard)) {
						ShowTypeSelectMenu();
					}
				} else {
					if (EditorGUI.DropdownButton(position, new GUIContent(hint), FocusType.Keyboard)) {
						var types = value.GetValidTypes();
						var menu = new GenericMenu();
						var props = value._GetValidMembers();
						menu.AddItem(new GUIContent("Reselect type..."), false, () => { EditorApplication.delayCall += () => { EditorApplication.delayCall += ShowTypeSelectMenu; }; });
						menu.AddSeparator("");
						menu.AddItem(
							new GUIContent($"{value.type.Name}.<None>"),
							values.Any(v => String.IsNullOrEmpty(v.memberName)),
							() => { OnMemberSelect(property, null); }
						);
						if (props.Any()) {
							foreach (var prop in props) {
								menu.AddItem(
									new GUIContent($"{value.type.Name}.{prop.Name}"),
									values.Any(v => v.memberName == prop.Name),
									() => { OnMemberSelect(property, prop); }
								);
							}
						}
						menu.DropDown(position);
					}
				}
				void ShowTypeSelectMenu() {
					var types = value.GetValidTypes();
					var menu = TypeSelectMenu(types.ToList(), values.Select(v => v.type), type => OnSelect(property, type), true);
					menu.DropDown(position);
				}
			}
		}

		protected void OnMemberSelect(SerializedProperty property, MemberInfo memberInfo) {
			var values = GetValues<T>(property);
			Undo.RecordObjects(property.serializedObject.targetObjects, $"Set {property.name}");
			foreach (var value in values) value.memberName = memberInfo?.Name;
			foreach (var target in property.serializedObject.targetObjects) EditorUtility.SetDirty(target);
			property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
		}

	}
}
#endif
