
#if UNITY_EDITOR
namespace Muc.Editor {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEditor.Build;
	using UnityEditorInternal;
	using UnityEngine;
	using UnityEngine.UIElements;
	using Object = UnityEngine.Object;


	/// <summary>
	/// <para>Store the settings for Muc that will be stored with the Unity Project.</para>
	/// <para>Not usable for runtime data settings storage.</para>
	/// </summary>
	[FilePath("ProjectSettings/MyUnityCollectionSettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public class MucProjectSettings : ScriptableSingleton<MucProjectSettings> {

		void OnDisable() => Save();
		public void Save() => Save(true);
		internal SerializedObject GetSerializedObject() => new(this);

	}


	internal class MucProjectSettingsProvider : SettingsProvider {

		SerializedObject serializedObject;

		static MucProjectSettings t => MucProjectSettings.instance;

		private class Styles {
			public static readonly GUIContent HideComponentsToggle = EditorGUIUtility.TrTextContent("Hide All Components", "Hides all Components added by MyUnityCollection in the Add Component Menu");
			public static readonly GUIContent HideGeneralComponentsToggle = EditorGUIUtility.TrTextContent("Hide Generic Components", $"Components from {nameof(Muc)}.{nameof(Muc.Components)} will not be shown in the Add Component Menu");
			public static readonly GUIContent HideSystemComponentsToggle = EditorGUIUtility.TrTextContent("Hide System Components", $"Components from {nameof(Muc)}.{nameof(Muc.Systems)} will not be shown in the Add Component Menu");
			public static readonly GUIContent HideScriptableObjectsToggle = EditorGUIUtility.TrTextContent("Hide All ScriptableObjects", "Hides all ScriptableObjects added by MyUnityCollection in the create menu");
		}

		public MucProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }

		public override void OnActivate(string searchContext, VisualElement rootElement) {
			MucProjectSettings.instance.Save();
			serializedObject = MucProjectSettings.instance.GetSerializedObject();
		}

		[SettingsProvider]
		public static SettingsProvider CreateSettingProvider() {
			return new MucProjectSettingsProvider("Project/MyUnityCollection", SettingsScope.Project, GetSearchKeywordsFromGUIContentProperties<Styles>());
		}

		public override void OnGUI(string searchContext) {
			serializedObject.Update();

			using (new GUILayout.HorizontalScope()) {
				GUILayout.Space(10);
				using (new GUILayout.VerticalScope()) {
					GUILayout.Space(10);

					EditorGUIUtility.labelWidth += 100;

					RefreshSymbols();
					SymbolToggle(HIDE_COMPONENTS_SYMBOL, Styles.HideComponentsToggle);
					SymbolToggle(HIDE_GENERAL_COMPONENTS_SYMBOL, Styles.HideGeneralComponentsToggle);
					SymbolToggle(HIDE_SYSTEM_COMPONENTS_SYMBOL, Styles.HideSystemComponentsToggle);
					SymbolToggle(HIDE_SCRIPTABLE_OBJECTS, Styles.HideScriptableObjectsToggle);

					EditorGUIUtility.labelWidth = 0;
				}
			}

			serializedObject.ApplyModifiedProperties();
		}


		private const string HIDE_COMPONENTS_SYMBOL = "MUC_HIDE_COMPONENTS";
		private const string HIDE_SYSTEM_COMPONENTS_SYMBOL = "MUC_HIDE_SYSTEM_COMPONENTS";
		private const string HIDE_GENERAL_COMPONENTS_SYMBOL = "MUC_HIDE_GENERAL_COMPONENTS";
		private const string HIDE_SCRIPTABLE_OBJECTS = "MUC_HIDE_SCRIPTABLE_OBJECTS";
		private readonly NamedBuildTarget BUILD_TARGET = NamedBuildTarget.Standalone;
		private List<string> symbols = null;


		private void SymbolToggle(string symbol, GUIContent label) {
			if (symbols.Contains(symbol)) {
				if (!EditorGUILayout.Toggle(label, true)) {
					symbols.Remove(symbol);
					PlayerSettings.SetScriptingDefineSymbols(BUILD_TARGET, String.Join(";", symbols));
				}
			} else {
				if (EditorGUILayout.Toggle(label, false)) {
					symbols.Add(symbol);
					PlayerSettings.SetScriptingDefineSymbols(BUILD_TARGET, String.Join(";", symbols));
				}
			}
		}

		private void RefreshSymbols() {
			symbols = PlayerSettings.GetScriptingDefineSymbols(BUILD_TARGET).Split(';').ToList();
		}

	}

}
#endif