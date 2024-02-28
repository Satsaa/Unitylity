
using System;
using UnityEngine;

namespace Unitylity.Editor {

	public class DisableAttribute : PropertyAttribute {
		public virtual bool disable => true;
	}

	public class EditableInPlayModeAttribute : DisableAttribute {
		public override bool disable => Application.isPlaying;
	}

	public class EditableInEditModeAttribute : DisableAttribute {
		public override bool disable => !Application.isPlaying;
	}


#if UNITY_EDITOR
	namespace Editor {

		using System;
		using System.Linq;
		using System.Collections.Generic;
		using UnityEngine;
		using UnityEditor;
		using UnityEditor.UI;
		using Object = UnityEngine.Object;
		using static Unitylity.Editor.PropertyUtil;
		using static Unitylity.Editor.EditorUtil;

		[CustomPropertyDrawer(typeof(DisableAttribute), true)]
		public class ReadOnlyEditorDrawer : PropertyDrawer {

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
				using (PropertyScope(position, label, property, out label)) {
					if (attribute is DisableAttribute ca) {
						using (DisabledScope(ca.disable)) {
							PropertyField(position, label, property);
						}
					} else {
						PropertyField(position, label, property);
					}
				}
			}
		}
	}

#endif
}