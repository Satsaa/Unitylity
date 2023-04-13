
// License: MIT
// Original Author: Tom Kail at Inkle
// Original Code: http://answers.unity.com/answers/1513032/view.html

namespace Unitylity.Editor {

	using UnityEngine;

	public class ShowEditorAttribute : PropertyAttribute {

		public ShowEditorAttribute() { }

	}


#if UNITY_EDITOR
	namespace Editor {

		using System;
		using System.Collections.Generic;
		using System.Linq;
		using UnityEditor;
		using UnityEngine;
		using static EditorUtil;
		using static PropertyUtil;
		using Object = UnityEngine.Object;

		/// <summary>
		/// Extends how ScriptableObject object references are displayed in the inspector
		/// Shows you all values under the object reference
		/// </summary>
		[CustomPropertyDrawer(typeof(ShowEditorAttribute), true)]
		public class ShowEditorAttributeDrawer : PropertyDrawer {

			private Editor editor;
			private Object[] values;
			private Rect lastRect;

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
				using (new EditorGUI.PropertyScope(position, label, property)) {

					var canExpand = property.objectReferenceValue != null;
					var foldoutRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
					if (canExpand) {
						property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, property.displayName, true);
					} else {
						foldoutRect.x += 10;
						EditorGUI.Foldout(foldoutRect, property.isExpanded, property.displayName, true, EditorStyles.label);
					}

					var propertyRect = new Rect(position.x + EditorGUIUtility.labelWidth + 2, position.y, position.width - EditorGUIUtility.labelWidth - 2, EditorGUIUtility.singleLineHeight);



					PropertyField(FieldRect(position), GUIContent.none, property);
					if (GUI.changed) property.serializedObject.ApplyModifiedProperties();

					if (canExpand && property.isExpanded) {
						using (IndentScope()) {

							if (lastRect.x != 0 && lastRect.y != 0 && lastRect.height != 0 && lastRect.width != 0) {

								GUI.Box(
									new Rect(
										0,
										position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1,
										Screen.width,
										lastRect.y + lastRect.height - position.y - position.height
									)
								, "");

								for (int i = 0; i < 2; i++) {
									GUI.Box(
										new Rect(
											0,
											position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1,
											indent,
											lastRect.y + lastRect.height - position.y - position.height
										)
									, "");
								}
							}

							if (!editor) {
								values ??= GetValues<Object>(property).ToArray();
								editor = Editor.CreateEditor(values);
							}
							var newValues = GetValues<Object>(property);
							if (values == null || newValues.SequenceEqual(values)) {
								Object.DestroyImmediate(editor);
								values = GetValues<Object>(property).ToArray();
								editor = Editor.CreateEditor(values);
							}
							if (editor) {
								editor.OnInspectorGUI();
								var newLastRect = GUILayoutUtility.GetLastRect();
								if (newLastRect.x != 0 && newLastRect.y != 0 && newLastRect.height != 0 && newLastRect.width != 0)
									lastRect = newLastRect;
							}

						}
					} else if (editor) {
						lastRect = default;
						Object.DestroyImmediate(editor);
					}
					property.serializedObject.ApplyModifiedProperties();
				}
			}

		}

	}
#endif
}