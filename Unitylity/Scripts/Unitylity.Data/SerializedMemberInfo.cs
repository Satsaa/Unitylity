
namespace Unitylity.Data {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEngine;

	[Serializable]
	public abstract class SerializedMemberInfo : SerializedType {

		protected virtual BindingFlags bindingFlags => BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

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

		public virtual string FormatMemberName(MemberInfo member) => member.Name;

#if UNITY_EDITOR
		public abstract MemberInfo _GetMemberInfo();
		public abstract IEnumerable<MemberInfo> _GetValidMembers();
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
		public override sealed MemberInfo _GetMemberInfo() => memberInfo;
		public override sealed IEnumerable<MemberInfo> _GetValidMembers() => GetValidMembers();
#endif
	}

}


#if UNITY_EDITOR
namespace Unitylity.Data.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;

	[CanEditMultipleObjects]
	[CustomPropertyDrawer(typeof(SerializedMemberInfo), true)]
	public abstract class SerializedMemberInfoDrawer<T> : SerializedTypeDrawer where T : SerializedMemberInfo {

		protected override string GetDropdownText(SerializedType _value) {
			var value = _value as T;
			return value == null
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
					) + $" ({value.type.Assembly.GetName().Name})");
		}

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
					text = GetDropdownText(value)
				};
				if (value.type == null) {
					if (EditorGUI.DropdownButton(position, new GUIContent(hint), FocusType.Keyboard)) {
						ShowTypeSelectMenu();
					}
				} else {
					if (EditorGUI.DropdownButton(position, new GUIContent(hint), FocusType.Keyboard)) {
						ShowMethodSelectMenu();
					}
				}
				void ShowTypeSelectMenu() {
					var types = value.GetValidTypes();
					var menu = TypeSelectMenu(types.ToList(), values.Select(v => v.type), type => { OnSelect(property, type); ShowMethodSelectMenu(); }, true);
					menu.DropDown(position);
				}
				void ShowMethodSelectMenu() {
					if (value.type == null) return;
					var types = value.GetValidTypes();
					var menu = new GenericMenu();
					var props = value._GetValidMembers().ToList();
					props.Sort((a, b) => a.ToString().CompareTo(b.ToString()));

					menu.AddItem(new GUIContent("Reselect type..."), false, () => { EditorApplication.delayCall += () => { EditorApplication.delayCall += ShowTypeSelectMenu; }; });
					menu.AddSeparator("");
					menu.AddItem(new GUIContent($"Selected type: {value.type.Name}"), false, () => { });
					menu.AddSeparator("");
					menu.AddItem(
						new GUIContent("<None>"),
						values.Any(v => String.IsNullOrEmpty(v.memberName)),
						() => { OnMemberSelect(property, null); }
					);
					if (props.Any()) {
						foreach (var prop in props) {
							var formatName = value.FormatMemberName(prop);
							menu.AddItem(
								new GUIContent(formatName),
								values.Any(v => {
									return v.memberName == formatName;
								}),
								() => { OnMemberSelect(property, prop); }
							);
						}
					}
					menu.DropDown(position);
				}
			}
		}

		protected void OnMemberSelect(SerializedProperty property, MemberInfo memberInfo) {
			var values = GetValues<T>(property);
			Undo.RecordObjects(property.serializedObject.targetObjects, $"Set {property.name}");
			foreach (var value in values) value.memberName = memberInfo == null ? default : value.FormatMemberName(memberInfo);
			foreach (var target in property.serializedObject.targetObjects) EditorUtility.SetDirty(target);
			property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
		}

	}

}
#endif