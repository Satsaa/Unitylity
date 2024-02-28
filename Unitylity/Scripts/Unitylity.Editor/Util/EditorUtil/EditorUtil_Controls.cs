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

		private static readonly int genericFieldHash = "s_GenericField".GetHashCode();

		private static GUIContent[] xywhContent = new GUIContent[4] { new("X"), new("Y"), new("W"), new("H") };
		private static GUIContent[] whContent = new GUIContent[2] { new("W"), new("H") };
		private static GUIContent[] xyzwContent = new GUIContent[4] { new("X"), new("Y"), new("Z"), new("W") };
		private static GUIContent[] xyzContent = new GUIContent[3] { new("X"), new("Y"), new("Z") };
		private static GUIContent[] xyContent = new GUIContent[2] { new("X"), new("Y") };
		private static float[] floatArray4 = new float[4];
		private static float[] floatArray3 = new float[3];
		private static float[] floatArray2 = new float[2];
		private static int[] intArray4 = new int[4];
		private static int[] intArray3 = new int[3];
		private static int[] intArray2 = new int[2];

		private static bool LabelHasContent(GUIContent label) {
			if (label == null) {
				return true;
			}
			return label.text != string.Empty || label.image != null;
		}

		public static Deferred ManualIndentScope() {
			var prev = EditorGUI.indentLevel;
			var prevIndent = indent;
			EditorGUI.indentLevel = 0;
			labelWidth -= prevIndent;
			return new Deferred(() => {
				EditorGUI.indentLevel = prev;
				labelWidth += prevIndent;
			});
		}

		public static Deferred ForceIndentScope(Rect position, out Rect indented) {
			indented = position;
			indented.xMin += indent;
			return LabelWidthScope(v => v - indent);
		}



		public static void Space(float space = spaceSize) {
			EditorGUILayout.Space(spaceSize);
		}



		public static void Label(GUIContent label) {
			EditorGUILayout.LabelField(label);
		}

		public static void Label(Rect position, GUIContent label) {
			EditorGUI.LabelField(position, label);
		}

		public static void BoldLabel(GUIContent label) {
			EditorGUILayout.LabelField(label, GUI.skin.GetStyle("BoldLabel"));
		}

		public static void BoldLabel(Rect position, GUIContent label) {
			EditorGUI.LabelField(position, label, GUI.skin.GetStyle("BoldLabel"));
		}


		/// <summary> Creates a label and returns a rect for a field </summary>
		public static Rect Prefix(GUIContent label) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				int id = GUIUtility.GetControlID(genericFieldHash, FocusType.Keyboard, indented);
				return EditorGUI.PrefixLabel(indented, id, label);
			}
		}
		/// <summary> Creates a label and returns a rect for a field </summary>
		public static Rect Prefix(Rect position, GUIContent label) {
			int id = GUIUtility.GetControlID(genericFieldHash, FocusType.Keyboard, position);
			return EditorGUI.PrefixLabel(position, id, label);
		}


		/// <summary> Creates a label with calculated width and returns a rect for a field </summary>
		public static Rect CalculatedPrefix(GUIContent label) {
			var labelWidth = EditorStyles.label.CalcSize(label).x;
			using (LabelWidthScope(labelWidth)) {
				return Prefix(label);
			}
		}
		/// <summary> Creates a label with calculated width and returns a rect for a field </summary>
		public static Rect CalculatedPrefix(Rect position, GUIContent label) {
			var labelWidth = EditorStyles.label.CalcSize(label).x;
			using (LabelWidthScope(labelWidth)) {
				return Prefix(position, label);
			}
		}




		public static bool ComponentHeader(bool expanded, Object[] targets) {
			var rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.GetStyle("IN Title"));
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.InspectorTitlebar(indented, expanded, targets, true);
			}
		}

		public static bool ComponentHeader(Rect position, bool expanded, Object[] targets) {
			return EditorGUI.InspectorTitlebar(position, expanded, targets, true);
		}



		public static void ScriptField(SerializedObject obj) {
			var prop = obj.FindProperty(script);
			using (DisabledScope()) PropertyField(prop);
		}

		public static void ScriptField(Rect position, SerializedObject obj) {
			var prop = obj.FindProperty(script);
			using (DisabledScope()) PropertyField(position, prop);
		}



		public static bool Foldout(Rect position, SerializedProperty property) {
			return property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, property.displayName, true);
		}

		public static bool Foldout(SerializedProperty property) {
			return property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.displayName, true);
		}

		public static bool Foldout(Rect position, SerializedProperty property, GUIContent content) {
			return property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, content, true);
		}

		public static bool Foldout(SerializedProperty property, GUIContent content) {
			return property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, content, true);
		}



		public static bool Foldout(Rect position, bool value, GUIContent content) {
			return EditorGUI.Foldout(position, value, content, true);
		}

		public static bool Foldout(bool value, GUIContent content) {
			return EditorGUILayout.Foldout(value, content, true);
		}

		public static bool Foldout(Rect position, bool value) {
			return EditorGUI.Foldout(position, value, GUIContent.none, true);
		}

		public static bool Foldout(bool value) {
			return EditorGUILayout.Foldout(value, GUIContent.none, true);
		}


		#region Complex Fields

		/// <summary> Property Field </summary>
		public static void MultiPropertyField(Rect position, IEnumerable<SerializedProperty> properties) {
			MultiPropertyField(position, properties.Select(v => new GUIContent(v.displayName, v.tooltip)), properties);
		}
		/// <summary> Property Field </summary>
		public static void MultiPropertyField(IEnumerable<SerializedProperty> properties) {
			MultiPropertyField(properties.Select(v => new GUIContent(v.displayName, v.tooltip)), properties);
		}
		/// <summary> Property Field </summary>
		public static void MultiPropertyField(Rect position, GUIContent label, IEnumerable<SerializedProperty> properties) {
			MultiPropertyField(position, label, properties.Select(v => new GUIContent(v.displayName, v.tooltip)), properties);
		}
		/// <summary> Property Field </summary>
		public static void MultiPropertyField(GUIContent label, IEnumerable<SerializedProperty> properties) {
			MultiPropertyField(label, properties.Select(v => new GUIContent(v.displayName, v.tooltip)), properties);
		}


		/// <summary> Property Field </summary>
		public static void MultiPropertyField(Rect position, IEnumerable<GUIContent> contents, IEnumerable<SerializedProperty> properties) {
			var indented = EditorGUI.IndentedRect(position);
			using (ManualIndentScope()) {
				DoMultiPropertyField(indented, contents, properties);
			}
		}
		/// <summary> Property Field </summary>
		public static void MultiPropertyField(IEnumerable<GUIContent> contents, IEnumerable<SerializedProperty> properties) {
			var rect = EditorGUILayout.GetControlRect();
			DoMultiPropertyField(rect, contents, properties);
		}
		/// <summary> Property Field </summary>
		public static void MultiPropertyField(Rect position, GUIContent label, IEnumerable<GUIContent> contents, IEnumerable<SerializedProperty> properties) {
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(position);
				var fieldRect = FieldRect(position);
				Prefix(labelRect, label);
				DoMultiPropertyField(fieldRect, contents, properties);
			} else {
				var indented = EditorGUI.IndentedRect(position);
				DoMultiPropertyField(indented, contents, properties);
			}
		}
		/// <summary> Property Field </summary>
		public static void MultiPropertyField(GUIContent label, IEnumerable<GUIContent> contents, IEnumerable<SerializedProperty> properties) {
			var rect = EditorGUILayout.GetControlRect();
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(rect);
				var fieldRect = FieldRect(rect);
				using (ManualIndentScope()) {
					Prefix(labelRect, label);
					DoMultiPropertyField(fieldRect, contents, properties);
				}
			} else {
				var indented = EditorGUI.IndentedRect(rect);
				DoMultiPropertyField(indented, contents, properties);
			}
		}

		private static void DoMultiPropertyField(Rect position, IEnumerable<GUIContent> contents, IEnumerable<SerializedProperty> properties) {
			if (contents.Count() < properties.Count()) throw new ArgumentException("Not enough contents for properties. There must be at least the same amount of GUIContents as there are properties.", nameof(contents));
			var propWidth = position.width / properties.Count() - ((properties.Count() - 1f) / properties.Count() * spacing);
			var propRect = position;
			var contentsEnumer = contents.GetEnumerator();
			foreach (var property in properties) {
				contentsEnumer.MoveNext();
				var content = contentsEnumer.Current;
				propRect.width = propWidth;
				var contentWidth = EditorStyles.label.CalcSize(content).x;
				using (LabelWidthScope(contentWidth)) {
					PropertyField(propRect, content, property);
				}
				propRect.x += propWidth + spacing;
			}
		}


		/// <summary> Property Field </summary>
		public static void PropertyField(Rect position, SerializedProperty property, bool includeChildren = false) {
			EditorGUI.PropertyField(position, property, includeChildren);
		}
		/// <summary> Property Field </summary>
		public static void PropertyField(SerializedProperty property, bool includeChildren = false) {
			EditorGUILayout.PropertyField(property, includeChildren);
		}
		/// <summary> Property Field </summary>
		public static void PropertyField(Rect position, GUIContent label, SerializedProperty property, bool includeChildren = false) {
			EditorGUI.PropertyField(position, property, label, includeChildren);
		}
		/// <summary> Property Field </summary>
		public static void PropertyField(GUIContent label, SerializedProperty property, bool includeChildren = false) {
			EditorGUILayout.PropertyField(property, label, includeChildren);
		}


		/// <summary> Property Field with restricted Type </summary>
		public static void PropertyField<T>(Rect position, SerializedProperty property) where T : Object {
			EditorGUI.ObjectField(position, property, typeof(T));
		}
		/// <summary> Property Field with restricted Type </summary>
		public static void PropertyField<T>(SerializedProperty property) where T : Object {
			EditorGUILayout.ObjectField(property, typeof(T));
		}
		/// <summary> Property Field with restricted Type </summary>
		public static void PropertyField<T>(Rect position, GUIContent label, SerializedProperty property) where T : Object {
			EditorGUI.ObjectField(position, property, typeof(T), label);
		}
		/// <summary> Property Field with restricted Type </summary>
		public static void PropertyField<T>(GUIContent label, SerializedProperty property) where T : Object {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				EditorGUI.ObjectField(indented, property, typeof(T), label);
			}
		}


		/// <summary> Property Field with restricted Type </summary>
		public static void PropertyField(Rect position, SerializedProperty property, Type type) {
			EditorGUI.ObjectField(position, property, type);
		}
		/// <summary> Property Field with restricted Type </summary>
		public static void PropertyField(SerializedProperty property, Type type) {
			EditorGUILayout.ObjectField(property, type);
		}
		/// <summary> Property Field with restricted Type </summary>
		public static void PropertyField(Rect position, GUIContent label, SerializedProperty property, Type type) {
			EditorGUI.ObjectField(position, property, type, label);
		}
		/// <summary> Property Field with restricted Type </summary>
		public static void PropertyField(GUIContent label, SerializedProperty property, Type type) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				EditorGUI.ObjectField(indented, property, type, label);
			}
		}



		public static bool DropdownField(Rect position, GUIContent content) {
			return EditorGUI.DropdownButton(position, content, FocusType.Keyboard);
		}

		public static bool DropdownField(GUIContent content) {
			return EditorGUILayout.DropdownButton(content, FocusType.Keyboard);
		}

		public static bool DropdownField(Rect position, GUIContent label, GUIContent content) {
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(position);
				var fieldRect = FieldRect(position);
				Prefix(labelRect, label);
				return EditorGUI.DropdownButton(fieldRect, content, FocusType.Keyboard);
			} else {
				var indented = EditorGUI.IndentedRect(position);
				return EditorGUI.DropdownButton(indented, content, FocusType.Keyboard);
			}
		}

		public static bool DropdownField(GUIContent label, GUIContent content) {
			var rect = EditorGUILayout.GetControlRect();
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(rect);
				var fieldRect = FieldRect(rect);
				using (ManualIndentScope()) {
					Prefix(labelRect, label);
					return EditorGUI.DropdownButton(fieldRect, content, FocusType.Keyboard);
				}
			} else {
				var indented = EditorGUI.IndentedRect(rect);
				return EditorGUI.DropdownButton(indented, content, FocusType.Keyboard);
			}
		}



		public static bool ButtonField(Rect position, GUIContent content) {
			using (ManualIndentScope()) {
				return GUI.Button(position, content);
			}
		}

		public static bool ButtonField(GUIContent content) {
			var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect());
			return GUI.Button(rect, content);
		}

		public static bool ButtonField(Rect position, GUIContent label, GUIContent content) {
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(position);
				var fieldRect = FieldRect(position);
				Prefix(labelRect, label);
				return GUI.Button(fieldRect, content);
			} else {
				var indented = EditorGUI.IndentedRect(position);
				return GUI.Button(indented, content);
			}
		}

		public static bool ButtonField(GUIContent label, GUIContent content) {
			var rect = EditorGUILayout.GetControlRect();
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(rect);
				var fieldRect = FieldRect(rect);
				using (ManualIndentScope()) {
					Prefix(labelRect, label);
					return GUI.Button(fieldRect, content);
				}
			} else {
				var indented = EditorGUI.IndentedRect(rect);
				return GUI.Button(indented, content);
			}
		}



		public static bool ButtonField(Rect position, GUIContent content, GUIStyle style) {
			using (ManualIndentScope()) {
				return GUI.Button(position, content, style);
			}
		}

		public static bool ButtonField(GUIContent content, GUIStyle style) {
			var rect = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect());
			return GUI.Button(rect, content, style);
		}

		public static bool ButtonField(Rect position, GUIContent label, GUIContent content, GUIStyle style) {
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(position);
				var fieldRect = FieldRect(position);
				Prefix(labelRect, label);
				return GUI.Button(fieldRect, content, style);
			} else {
				var indented = EditorGUI.IndentedRect(position);
				return GUI.Button(indented, content, style);
			}
		}

		public static bool ButtonField(GUIContent label, GUIContent content, GUIStyle style) {
			var rect = EditorGUILayout.GetControlRect();
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(rect);
				var fieldRect = FieldRect(rect);
				using (ManualIndentScope()) {
					Prefix(labelRect, label);
					return GUI.Button(fieldRect, content, style);
				}
			} else {
				var indented = EditorGUI.IndentedRect(rect);
				return GUI.Button(indented, content, style);
			}
		}



		public static void HelpBoxField(Rect position, string message, MessageType type) {
			EditorGUI.HelpBox(position, message, type);
		}

		public static void HelpBoxField(string message, MessageType type) {
			EditorGUILayout.HelpBox(message, type);
		}

		public static void HelpBoxField(Rect position, GUIContent label, string message, MessageType type) {
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(position);
				var fieldRect = FieldRect(position);
				Prefix(labelRect, label);
				EditorGUI.HelpBox(fieldRect, message, type);
			} else {
				var indented = EditorGUI.IndentedRect(position);
				EditorGUI.HelpBox(indented, message, type);
			}
		}

		public static void HelpBoxField(GUIContent label, string message, MessageType type) {
			if (LabelHasContent(label)) {
				var rect = EditorGUILayout.GetControlRect();
				var labelRect = LabelRect(rect);
				var fieldRect = FieldRect(rect);
				using (ManualIndentScope()) {
					Prefix(labelRect, label);
					EditorGUI.HelpBox(fieldRect, message, type);
				}
			} else {
				var rect = EditorGUILayout.GetControlRect(false, helpBoxHeight);
				var indented = EditorGUI.IndentedRect(rect);
				EditorGUI.HelpBox(indented, message, type);
			}
		}



		public static void MinMaxSliderField(Rect position, ref float minValue, ref float maxValue, float minLimit, float maxLimit) {
			EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, minLimit, maxLimit);
		}

		public static void MinMaxSliderField(ref float minValue, ref float maxValue, float minLimit, float maxLimit) {
			EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, minLimit, maxLimit);
		}

		public static void MinMaxSliderField(Rect position, GUIContent label, ref float minValue, ref float maxValue, float minLimit, float maxLimit) {
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(position);
				var fieldRect = FieldRect(position);
				Prefix(labelRect, label);
				EditorGUI.MinMaxSlider(fieldRect, ref minValue, ref maxValue, minLimit, maxLimit);
			} else {
				var indented = EditorGUI.IndentedRect(position);
				EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, minLimit, maxLimit);
			}
		}

		public static void MinMaxSliderField(GUIContent label, ref float minValue, ref float maxValue, float minLimit, float maxLimit) {
			var rect = EditorGUILayout.GetControlRect();
			if (LabelHasContent(label)) {
				var labelRect = LabelRect(rect);
				var fieldRect = FieldRect(rect);
				using (ManualIndentScope()) {
					Prefix(labelRect, label);
					EditorGUI.MinMaxSlider(fieldRect, ref minValue, ref maxValue, minLimit, maxLimit);
				}
			} else {
				var indented = EditorGUI.IndentedRect(rect);
				EditorGUI.MinMaxSlider(indented, ref minValue, ref maxValue, minLimit, maxLimit);
			}
		}



		public static int SliderField(Rect position, int value, int leftValue, int rightValue) {
			return EditorGUI.IntSlider(position, value, leftValue, rightValue);
		}

		public static int SliderField(int value, int leftValue, int rightValue) {
			return EditorGUILayout.IntSlider(value, leftValue, rightValue);
		}

		public static int SliderField(Rect position, GUIContent label, int value, int leftValue, int rightValue) {
			return EditorGUI.IntSlider(position, label, value, leftValue, rightValue);
		}

		public static int SliderField(GUIContent label, int value, int leftValue, int rightValue) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.IntSlider(indented, label, value, leftValue, rightValue);
			}
		}



		public static float SliderField(Rect position, float value, float leftValue, float rightValue) {
			return EditorGUI.Slider(position, value, leftValue, rightValue);
		}

		public static float SliderField(float value, float leftValue, float rightValue) {
			return EditorGUILayout.Slider(value, leftValue, rightValue);
		}

		public static float SliderField(Rect position, GUIContent label, float value, float leftValue, float rightValue) {
			return EditorGUI.Slider(position, label, value, leftValue, rightValue);
		}

		public static float SliderField(GUIContent label, float value, float leftValue, float rightValue) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.Slider(indented, label, value, leftValue, rightValue);
			}
		}

		#endregion

		#region Specialized Fields

		public static string PasswordField(Rect position, string value) {
			return EditorGUI.PasswordField(position, value);
		}

		public static string PasswordField(string value) {
			return EditorGUILayout.PasswordField(value);
		}

		public static string PasswordField(Rect position, GUIContent label, string value) {
			return EditorGUI.PasswordField(position, label, value);
		}

		public static string PasswordField(GUIContent label, string value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.PasswordField(indented, label, value);
			}
		}



		public static int LayerField(Rect position, int layer) {
			return EditorGUI.LayerField(position, layer);
		}

		public static int LayerField(int layer) {
			return EditorGUILayout.LayerField(layer);
		}

		public static int LayerField(Rect position, GUIContent label, int layer) {
			return EditorGUI.LayerField(position, label, layer);
		}

		public static int LayerField(GUIContent label, int layer) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.LayerField(indented, label, layer);
			}
		}



		public static string TagField(Rect position, string tag) {
			return EditorGUI.TagField(position, tag);
		}

		public static string TagField(string tag) {
			return EditorGUILayout.TagField(tag);
		}

		public static string TagField(Rect position, GUIContent label, string tag) {
			return EditorGUI.TagField(position, label, tag);
		}

		public static string TagField(GUIContent label, string tag) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.TagField(indented, label, tag);
			}
		}

		#endregion

		#region Delayed Fields

		/// <summary> Delayed String Field </summary>
		public static string DelayedField(Rect position, string value) {
			return EditorGUI.DelayedTextField(position, value);
		}
		/// <summary> Delayed String Field </summary>
		public static string DelayedField(string value) {
			return EditorGUILayout.DelayedTextField(value);
		}
		/// <summary> Delayed String Field </summary>
		public static string DelayedField(Rect position, GUIContent label, string value) {
			return EditorGUI.DelayedTextField(position, label, value);
		}
		/// <summary> Delayed String Field </summary>
		public static string DelayedField(GUIContent label, string value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.DelayedTextField(indented, label, value);
			}
		}


		/// <summary> Delayed Int Field </summary>
		public static int DelayedField(Rect position, int value) {
			return EditorGUI.DelayedIntField(position, value);
		}
		/// <summary> Delayed Int Field </summary>
		public static int DelayedField(int value) {
			return EditorGUILayout.DelayedIntField(value);
		}
		/// <summary> Delayed Int Field </summary>
		public static int DelayedField(Rect position, GUIContent label, int value) {
			return EditorGUI.DelayedIntField(position, label, value);
		}
		/// <summary> Delayed Int Field </summary>
		public static int DelayedField(GUIContent label, int value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.DelayedIntField(indented, label, value);
			}
		}


		/// <summary> Delayed Float Field </summary>
		public static float DelayedField(Rect position, float value) {
			return EditorGUI.DelayedFloatField(position, value);
		}
		/// <summary> Delayed Float Field </summary>
		public static float DelayedField(float value) {
			return EditorGUILayout.DelayedFloatField(value);
		}
		/// <summary> Delayed Float Field </summary>
		public static float DelayedField(Rect position, GUIContent label, float value) {
			return EditorGUI.DelayedFloatField(position, label, value);
		}
		/// <summary> Delayed Float Field </summary>
		public static float DelayedField(GUIContent label, float value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.DelayedFloatField(indented, label, value);
			}
		}


		/// <summary> Delayed Double Field </summary>
		public static double DelayedField(Rect position, double value) {
			return EditorGUI.DelayedDoubleField(position, value);
		}
		/// <summary> Delayed Double Field </summary>
		public static double DelayedField(double value) {
			return EditorGUILayout.DelayedDoubleField(value);
		}
		/// <summary> Delayed Double Field </summary>
		public static double DelayedField(Rect position, GUIContent label, double value) {
			return EditorGUI.DelayedDoubleField(position, label, value);
		}
		/// <summary> Delayed Double Field </summary>
		public static double DelayedField(GUIContent label, double value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.DelayedDoubleField(indented, label, value);
			}
		}

		#endregion

		#region Value Fields

		/// <summary> Enum Field </summary>
		public static T Field<T>(Rect position, T enumValue) where T : Enum {
			if (typeof(T).IsDefined(typeof(FlagsAttribute), false)) {
				return (T)EditorGUI.EnumFlagsField(position, enumValue);
			} else {
				return (T)EditorGUI.EnumPopup(position, enumValue);
			}
		}
		/// <summary> Enum Field </summary>
		public static T Field<T>(T enumValue) where T : Enum {
			if (typeof(T).IsDefined(typeof(FlagsAttribute), false)) {
				return (T)EditorGUILayout.EnumFlagsField(enumValue);
			} else {
				return (T)EditorGUILayout.EnumPopup(enumValue);
			}
		}
		/// <summary> Enum Field </summary>
		public static T Field<T>(Rect position, GUIContent label, T enumValue) where T : Enum {
			if (typeof(T).IsDefined(typeof(FlagsAttribute), false)) {
				return (T)EditorGUI.EnumFlagsField(position, label, enumValue);
			} else {
				return (T)EditorGUI.EnumPopup(position, label, enumValue);
			}
		}
		/// <summary> Enum Field </summary>
		public static T Field<T>(GUIContent label, T enumValue) where T : Enum {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				if (typeof(T).IsDefined(typeof(FlagsAttribute), false)) {
					return (T)EditorGUI.EnumFlagsField(indented, label, enumValue);
				} else {
					return (T)EditorGUI.EnumPopup(indented, label, enumValue);
				}
			}
		}


		/// <summary> Object Field </summary>
		public static T Field<T>(Rect position, T value, bool allowSceneObjects) where T : Object {
			return EditorGUI.ObjectField(position, value, typeof(T), allowSceneObjects) as T;
		}
		/// <summary> Object Field </summary>
		public static T Field<T>(T value, bool allowSceneObjects) where T : Object {
			return EditorGUILayout.ObjectField(value, typeof(T), allowSceneObjects) as T;
		}
		/// <summary> Object Field </summary>
		public static T Field<T>(Rect position, GUIContent label, T value, bool allowSceneObjects) where T : Object {
			return EditorGUI.ObjectField(position, label, value, typeof(T), allowSceneObjects) as T;
		}
		/// <summary> Object Field </summary>
		public static T Field<T>(GUIContent label, T value, bool allowSceneObjects) where T : Object {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.ObjectField(indented, label, value, typeof(T), allowSceneObjects) as T;
			}
		}


		/// <summary> Boolean Field </summary>
		public static bool Field(Rect position, bool value) {
			return EditorGUI.Toggle(position, value);
		}
		/// <summary> Boolean Field </summary>
		public static bool Field(bool value) {
			return EditorGUILayout.Toggle(value);
		}
		/// <summary> Boolean Field </summary>
		public static bool Field(Rect position, GUIContent label, bool value) {
			return EditorGUI.Toggle(position, label, value);
		}
		/// <summary> Boolean Field </summary>
		public static bool Field(GUIContent label, bool value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.Toggle(indented, label, value);
			}
		}


		/// <summary> Rect Field </summary>
		public static Rect Field(Rect position, Rect value) {
			floatArray4[0] = value.x;
			floatArray4[1] = value.y;
			floatArray4[2] = value.width;
			floatArray4[3] = value.height;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(position, xywhContent, floatArray4);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray4[0];
				value.y = floatArray4[1];
				value.width = floatArray4[2];
				value.height = floatArray4[3];
			}
			return value;
		}
		/// <summary> Rect Field </summary>
		public static Rect Field(Rect value) {
			var rect = EditorGUILayout.GetControlRect();
			floatArray4[0] = value.x;
			floatArray4[1] = value.y;
			floatArray4[2] = value.width;
			floatArray4[3] = value.height;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(rect, xywhContent, floatArray4);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray4[0];
				value.y = floatArray4[1];
				value.width = floatArray4[2];
				value.height = floatArray4[3];
			}
			return value;
		}
		/// <summary> Rect Field </summary>
		public static Rect Field(Rect position, GUIContent label, Rect value) {
			floatArray4[0] = value.x;
			floatArray4[1] = value.y;
			floatArray4[2] = value.width;
			floatArray4[3] = value.height;
			EditorGUI.BeginChangeCheck();
			var fieldRect = Prefix(position, label);
			EditorGUI.MultiFloatField(fieldRect, xywhContent, floatArray4);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray4[0];
				value.y = floatArray4[1];
				value.width = floatArray4[2];
				value.height = floatArray4[3];
			}
			return value;
		}
		/// <summary> Rect Field </summary>
		public static Rect Field(GUIContent label, Rect value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				floatArray4[0] = value.x;
				floatArray4[1] = value.y;
				floatArray4[2] = value.width;
				floatArray4[3] = value.height;
				EditorGUI.BeginChangeCheck();
				var fieldRect = Prefix(indented, label);
				EditorGUI.MultiFloatField(fieldRect, xywhContent, floatArray4);
				if (EditorGUI.EndChangeCheck()) {
					value.x = floatArray4[0];
					value.y = floatArray4[1];
					value.width = floatArray4[2];
					value.height = floatArray4[3];
				}
				return value;
			}
		}


		/// <summary> Rect Field </summary>
		public static RectInt Field(Rect position, RectInt value) {
			intArray4[0] = value.x;
			intArray4[1] = value.y;
			intArray4[2] = value.width;
			intArray4[3] = value.height;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiIntField(position, xywhContent, intArray4);
			if (EditorGUI.EndChangeCheck()) {
				value.x = intArray4[0];
				value.y = intArray4[1];
				value.width = intArray4[2];
				value.height = intArray4[3];
			}
			return value;
		}
		/// <summary> Rect Field </summary>
		public static RectInt Field(RectInt value) {
			var rect = EditorGUILayout.GetControlRect();
			intArray4[0] = value.x;
			intArray4[1] = value.y;
			intArray4[2] = value.width;
			intArray4[3] = value.height;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiIntField(rect, xywhContent, intArray4);
			if (EditorGUI.EndChangeCheck()) {
				value.x = intArray4[0];
				value.y = intArray4[1];
				value.width = intArray4[2];
				value.height = intArray4[3];
			}
			return value;
		}
		/// <summary> Rect Field </summary>
		public static RectInt Field(Rect position, GUIContent label, RectInt value) {
			intArray4[0] = value.x;
			intArray4[1] = value.y;
			intArray4[2] = value.width;
			intArray4[3] = value.height;
			EditorGUI.BeginChangeCheck();
			var fieldRect = Prefix(position, label);
			EditorGUI.MultiIntField(fieldRect, xywhContent, intArray4);
			if (EditorGUI.EndChangeCheck()) {
				value.x = intArray4[0];
				value.y = intArray4[1];
				value.width = intArray4[2];
				value.height = intArray4[3];
			}
			return value;
		}
		/// <summary> Rect Field </summary>
		public static RectInt Field(GUIContent label, RectInt value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				intArray4[0] = value.x;
				intArray4[1] = value.y;
				intArray4[2] = value.width;
				intArray4[3] = value.height;
				EditorGUI.BeginChangeCheck();
				var fieldRect = Prefix(indented, label);
				EditorGUI.MultiIntField(fieldRect, xywhContent, intArray4);
				if (EditorGUI.EndChangeCheck()) {
					value.x = intArray4[0];
					value.y = intArray4[1];
					value.width = intArray4[2];
					value.height = intArray4[3];
				}
				return value;
			}
		}


		/// <summary> Bounds Field </summary>
		public static Bounds Field(Rect position, Bounds value) {
			return EditorGUI.BoundsField(position, value);
		}
		/// <summary> Bounds Field </summary>
		public static Bounds Field(Bounds value) {
			return EditorGUILayout.BoundsField(value);
		}
		/// <summary> Bounds Field </summary>
		public static Bounds Field(Rect position, GUIContent label, Bounds value) {
			return EditorGUI.BoundsField(position, label, value);
		}
		/// <summary> Bounds Field </summary>
		public static Bounds Field(GUIContent label, Bounds value) {
			var rect = EditorGUILayout.GetControlRect(false, EditorGUI.GetPropertyHeight(SerializedPropertyType.Bounds, label));
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.BoundsField(indented, label, value);
			}
		}


		/// <summary> BoundsInt Field </summary>
		public static BoundsInt Field(Rect position, BoundsInt value) {
			return EditorGUI.BoundsIntField(position, value);
		}
		/// <summary> BoundsInt Field </summary>
		public static BoundsInt Field(BoundsInt value) {
			return EditorGUILayout.BoundsIntField(value);
		}
		/// <summary> BoundsInt Field </summary>
		public static BoundsInt Field(Rect position, GUIContent label, BoundsInt value) {
			return EditorGUI.BoundsIntField(position, label, value);
		}
		/// <summary> BoundsInt Field </summary>
		public static BoundsInt Field(GUIContent label, BoundsInt value) {
			var rect = EditorGUILayout.GetControlRect(false, EditorGUI.GetPropertyHeight(SerializedPropertyType.BoundsInt, label));
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.BoundsIntField(indented, label, value);
			}
		}


		/// <summary> Color Field </summary>
		public static Color Field(Rect position, Color value) {
			return EditorGUI.ColorField(position, value);
		}
		/// <summary> Color Field </summary>
		public static Color Field(Color value) {
			return EditorGUILayout.ColorField(value);
		}
		/// <summary> Color Field </summary>
		public static Color Field(Rect position, GUIContent label, Color value) {
			return EditorGUI.ColorField(position, label, value);
		}
		/// <summary> Color Field </summary>
		public static Color Field(GUIContent label, Color value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.ColorField(indented, label, value);
			}
		}


		/// <summary> AnimationCurve Field </summary>
		public static AnimationCurve Field(Rect position, AnimationCurve value) {
			return EditorGUI.CurveField(position, value);
		}
		/// <summary> AnimationCurve Field </summary>
		public static AnimationCurve Field(AnimationCurve value) {
			return EditorGUILayout.CurveField(value);
		}
		/// <summary> AnimationCurve Field </summary>
		public static AnimationCurve Field(Rect position, GUIContent label, AnimationCurve value) {
			return EditorGUI.CurveField(position, label, value);
		}
		/// <summary> AnimationCurve Field </summary>
		public static AnimationCurve Field(GUIContent label, AnimationCurve value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.CurveField(indented, label, value);
			}
		}


		/// <summary> Gradient Field </summary>
		public static Gradient Field(Rect position, Gradient value) {
			return EditorGUI.GradientField(position, value);
		}
		/// <summary> Gradient Field </summary>
		public static Gradient Field(Gradient value) {
			return EditorGUILayout.GradientField(value);
		}
		/// <summary> Gradient Field </summary>
		public static Gradient Field(Rect position, GUIContent label, Gradient value) {
			return EditorGUI.GradientField(position, label, value);
		}
		/// <summary> Gradient Field </summary>
		public static Gradient Field(GUIContent label, Gradient value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.GradientField(indented, label, value);
			}
		}


		/// <summary> Vector2 Field </summary>
		public static Vector2 Field(Rect position, Vector2 value) {
			floatArray2[0] = value.x;
			floatArray2[1] = value.y;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(position, xyContent, floatArray2);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray2[0];
				value.y = floatArray2[1];
			}
			return value;
		}
		/// <summary> Vector2 Field </summary>
		public static Vector2 Field(Vector2 value) {
			var rect = EditorGUILayout.GetControlRect();
			floatArray2[0] = value.x;
			floatArray2[1] = value.y;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(rect, xyContent, floatArray2);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray2[0];
				value.y = floatArray2[1];
			}
			return value;
		}
		/// <summary> Vector2 Field </summary>
		public static Vector2 Field(Rect position, GUIContent label, Vector2 value) {
			floatArray2[0] = value.x;
			floatArray2[1] = value.y;
			EditorGUI.BeginChangeCheck();
			var fieldRect = Prefix(position, label);
			EditorGUI.MultiFloatField(fieldRect, xyContent, floatArray2);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray2[0];
				value.y = floatArray2[1];
			}
			return value;
		}
		/// <summary> Vector2 Field </summary>
		public static Vector2 Field(GUIContent label, Vector2 value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				floatArray2[0] = value.x;
				floatArray2[1] = value.y;
				EditorGUI.BeginChangeCheck();
				var fieldRect = Prefix(indented, label);
				EditorGUI.MultiFloatField(fieldRect, xyContent, floatArray2);
				if (EditorGUI.EndChangeCheck()) {
					value.x = floatArray2[0];
					value.y = floatArray2[1];
				}
				return value;
			}
		}


		/// <summary> Vector2Int Field </summary>
		public static Vector2Int Field(Rect position, Vector2Int value) {
			intArray2[0] = value.x;
			intArray2[1] = value.y;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiIntField(position, xyContent, intArray2);
			if (EditorGUI.EndChangeCheck()) {
				value.x = intArray2[0];
				value.y = intArray2[1];
			}
			return value;
		}
		/// <summary> Vector2Int Field </summary>
		public static Vector2Int Field(Vector2Int value) {
			var rect = EditorGUILayout.GetControlRect();

			intArray2[0] = value.x;
			intArray2[1] = value.y;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiIntField(rect, xyContent, intArray2);
			if (EditorGUI.EndChangeCheck()) {
				value.x = intArray2[0];
				value.y = intArray2[1];
			}
			return value;
		}
		/// <summary> Vector2Int Field </summary>
		public static Vector2Int Field(Rect position, GUIContent label, Vector2Int value) {
			intArray2[0] = value.x;
			intArray2[1] = value.y;
			EditorGUI.BeginChangeCheck();
			var fieldRect = Prefix(position, label);
			EditorGUI.MultiIntField(fieldRect, xyContent, intArray2);
			if (EditorGUI.EndChangeCheck()) {
				value.x = intArray2[0];
				value.y = intArray2[1];
			}
			return value;
		}
		/// <summary> Vector2Int Field </summary>
		public static Vector2Int Field(GUIContent label, Vector2Int value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				intArray2[0] = value.x;
				intArray2[1] = value.y;
				EditorGUI.BeginChangeCheck();
				var fieldRect = Prefix(indented, label);
				EditorGUI.MultiIntField(fieldRect, xyContent, intArray2);
				if (EditorGUI.EndChangeCheck()) {
					value.x = intArray2[0];
					value.y = intArray2[1];
				}
				return value;
			}
		}


		/// <summary> Vector3 Field </summary>
		public static Vector3 Field(Rect position, Vector3 value) {
			floatArray3[0] = value.x;
			floatArray3[1] = value.y;
			floatArray3[2] = value.z;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(position, xyzContent, floatArray3);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray3[0];
				value.y = floatArray3[1];
				value.z = floatArray3[2];
			}
			return value;
		}
		/// <summary> Vector3 Field </summary>
		public static Vector3 Field(Vector3 value) {
			var rect = EditorGUILayout.GetControlRect();
			floatArray3[0] = value.x;
			floatArray3[1] = value.y;
			floatArray3[2] = value.z;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(rect, xyzContent, floatArray3);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray3[0];
				value.y = floatArray3[1];
				value.z = floatArray3[2];
			}
			return value;
		}
		/// <summary> Vector3 Field </summary>
		public static Vector3 Field(Rect position, GUIContent label, Vector3 value) {
			floatArray3[0] = value.x;
			floatArray3[1] = value.y;
			floatArray3[2] = value.z;
			EditorGUI.BeginChangeCheck();
			var fieldRect = Prefix(position, label);
			EditorGUI.MultiFloatField(fieldRect, xyzContent, floatArray3);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray3[0];
				value.y = floatArray3[1];
				value.z = floatArray3[2];
			}
			return value;
		}
		/// <summary> Vector3 Field </summary>
		public static Vector3 Field(GUIContent label, Vector3 value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				floatArray3[0] = value.x;
				floatArray3[1] = value.y;
				floatArray3[2] = value.z;
				EditorGUI.BeginChangeCheck();
				var fieldRect = Prefix(indented, label);
				EditorGUI.MultiFloatField(fieldRect, xyzContent, floatArray3);
				if (EditorGUI.EndChangeCheck()) {
					value.x = floatArray3[0];
					value.y = floatArray3[1];
					value.z = floatArray3[2];
				}
				return value;
			}
		}


		/// <summary> Vector3Int Field </summary>
		public static Vector3Int Field(Rect position, Vector3Int value) {
			intArray3[0] = value.x;
			intArray3[1] = value.y;
			intArray3[2] = value.z;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiIntField(position, xyzContent, intArray3);
			if (EditorGUI.EndChangeCheck()) {
				value.x = intArray3[0];
				value.y = intArray3[1];
				value.z = intArray3[2];
			}
			return value;
		}
		/// <summary> Vector3Int Field </summary>
		public static Vector3Int Field(Vector3Int value) {
			var rect = EditorGUILayout.GetControlRect();
			intArray3[0] = value.x;
			intArray3[1] = value.y;
			intArray3[2] = value.z;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiIntField(rect, xyzContent, intArray3);
			if (EditorGUI.EndChangeCheck()) {
				value.x = intArray3[0];
				value.y = intArray3[1];
				value.z = intArray3[2];
			}
			return value;
		}
		/// <summary> Vector3Int Field </summary>
		public static Vector3Int Field(Rect position, GUIContent label, Vector3Int value) {
			intArray3[0] = value.x;
			intArray3[1] = value.y;
			intArray3[2] = value.z;
			EditorGUI.BeginChangeCheck();
			var fieldRect = Prefix(position, label);
			EditorGUI.MultiIntField(fieldRect, xyzContent, intArray3);
			if (EditorGUI.EndChangeCheck()) {
				value.x = intArray3[0];
				value.y = intArray3[1];
				value.z = intArray3[2];
			}
			return value;
		}
		/// <summary> Vector3Int Field </summary>
		public static Vector3Int Field(GUIContent label, Vector3Int value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				intArray3[0] = value.x;
				intArray3[1] = value.y;
				intArray3[2] = value.z;
				EditorGUI.BeginChangeCheck();
				var fieldRect = Prefix(indented, label);
				EditorGUI.MultiIntField(fieldRect, xyzContent, intArray3);
				if (EditorGUI.EndChangeCheck()) {
					value.x = intArray3[0];
					value.y = intArray3[1];
					value.z = intArray3[2];
				}
				return value;
			}
		}


		/// <summary> Vector4 Field </summary>
		public static Vector4 Field(Rect position, Vector4 value) {
			floatArray4[0] = value.x;
			floatArray4[1] = value.y;
			floatArray4[2] = value.z;
			floatArray4[3] = value.w;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(position, xyzwContent, floatArray4);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray4[0];
				value.y = floatArray4[1];
				value.z = floatArray4[2];
				value.w = floatArray4[3];
			}
			return value;
		}
		/// <summary> Vector4 Field </summary>
		public static Vector4 Field(Vector4 value) {
			var rect = EditorGUILayout.GetControlRect();
			floatArray4[0] = value.x;
			floatArray4[1] = value.y;
			floatArray4[2] = value.z;
			floatArray4[3] = value.w;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(rect, xyzwContent, floatArray4);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray4[0];
				value.y = floatArray4[1];
				value.z = floatArray4[2];
				value.w = floatArray4[3];
			}
			return value;
		}
		/// <summary> Vector4 Field </summary>
		public static Vector4 Field(Rect position, GUIContent label, Vector4 value) {
			floatArray4[0] = value.x;
			floatArray4[1] = value.y;
			floatArray4[2] = value.z;
			floatArray4[3] = value.w;
			EditorGUI.BeginChangeCheck();
			var fieldRect = Prefix(position, label);
			EditorGUI.MultiFloatField(fieldRect, xyzwContent, floatArray4);
			if (EditorGUI.EndChangeCheck()) {
				value.x = floatArray4[0];
				value.y = floatArray4[1];
				value.z = floatArray4[2];
				value.w = floatArray4[3];
			}
			return value;
		}
		/// <summary> Vector4 Field </summary>
		public static Vector4 Field(GUIContent label, Vector4 value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				floatArray4[0] = value.x;
				floatArray4[1] = value.y;
				floatArray4[2] = value.z;
				floatArray4[3] = value.w;
				EditorGUI.BeginChangeCheck();
				var fieldRect = Prefix(indented, label);
				EditorGUI.MultiFloatField(fieldRect, xyzwContent, floatArray4);
				if (EditorGUI.EndChangeCheck()) {
					value.x = floatArray4[0];
					value.y = floatArray4[1];
					value.z = floatArray4[2];
					value.w = floatArray4[3];
				}
				return value;
			}
		}


		/// <summary> String Field </summary>
		public static string Field(Rect position, string value) {
			return EditorGUI.TextField(position, value);
		}
		/// <summary> String Field </summary>
		public static string Field(string value) {
			return EditorGUILayout.TextField(value);
		}
		/// <summary> String Field </summary>
		public static string Field(Rect position, GUIContent label, string value) {
			return EditorGUI.TextField(position, label, value);
		}
		/// <summary> String Field </summary>
		public static string Field(GUIContent label, string value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.TextField(indented, label, value);
			}
		}


		/// <summary> Int Field </summary>
		public static int Field(Rect position, int value) {
			using (ManualIndentScope()) {
				return EditorGUI.IntField(position, value);
			}
		}
		/// <summary> Int Field </summary>
		public static int Field(int value) {
			return EditorGUILayout.IntField(value);
		}
		/// <summary> Int Field </summary>
		public static int Field(Rect position, GUIContent label, int value) {
			return EditorGUI.IntField(position, label, value);
		}
		/// <summary> Int Field </summary>
		public static int Field(GUIContent label, int value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.IntField(indented, label, value);
			}
		}


		/// <summary> Float Field </summary>
		public static float Field(Rect position, float value) {
			return EditorGUI.FloatField(position, value);
		}
		/// <summary> Float Field </summary>
		public static float Field(float value) {
			return EditorGUILayout.FloatField(value);
		}
		/// <summary> Float Field </summary>
		public static float Field(Rect position, GUIContent label, float value) {
			return EditorGUI.FloatField(position, label, value);
		}
		/// <summary> Float Field </summary>
		public static float Field(GUIContent label, float value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.FloatField(indented, label, value);
			}
		}


		/// <summary> Long Field </summary>
		public static long Field(Rect position, long value) {
			return EditorGUI.LongField(position, value);
		}
		/// <summary> Long Field </summary>
		public static long Field(long value) {
			return EditorGUILayout.LongField(value);
		}
		/// <summary> Long Field </summary>
		public static long Field(Rect position, GUIContent label, long value) {
			return EditorGUI.LongField(position, label, value);
		}
		/// <summary> Long Field </summary>
		public static long Field(GUIContent label, long value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.LongField(indented, label, value);
			}
		}


		/// <summary> Double Field </summary>
		public static double Field(Rect position, double value) {
			return EditorGUI.DoubleField(position, value);
		}
		/// <summary> Double Field </summary>
		public static double Field(double value) {
			return EditorGUILayout.DoubleField(value);
		}
		/// <summary> Double Field </summary>
		public static double Field(Rect position, GUIContent label, double value) {
			return EditorGUI.DoubleField(position, label, value);
		}
		/// <summary> Double Field </summary>
		public static double Field(GUIContent label, double value) {
			var rect = EditorGUILayout.GetControlRect();
			var indented = EditorGUI.IndentedRect(rect);
			using (ManualIndentScope()) {
				return EditorGUI.DoubleField(indented, label, value);
			}
		}

		#endregion

	}

}
#endif