
#if UNITY_EDITOR
namespace Unitylity.Editor {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEditor.Build;
	using UnityEditorInternal;
	using UnityEngine;
	using UnityEngine.UIElements;
	using static Unitylity.Editor.EditorUtil;
	using Object = UnityEngine.Object;


	/// <summary>
	/// <para>Store the settings for Unitylity that will be stored with the Unity Project.</para>
	/// <para>Not usable for runtime data settings storage.</para>
	/// </summary>
	[FilePath("ProjectSettings/UnitylitySettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public class UnitylitySettings : ScriptableSingleton<UnitylitySettings> {

		void OnDisable() => Save();
		public void Save() => Save(true);
		internal SerializedObject GetSerializedObject() => new(this);

	}

	internal enum State {
		Enabled = 0,
		Hidden = 1,
		Disabled = 2
	}

	internal enum VisualState {
		Visible = 0,
		Hidden = 1,
	}

	internal class UnitylitySettingsProvider : SettingsProvider {

		SerializedObject serializedObject;

		static UnitylitySettings t => UnitylitySettings.instance;

		private class Styles {
			public static readonly GUIContent GeneralComponentsToggle = EditorGUIUtility.TrTextContent("Generic Assets", $"Assets from Unitylity.Components except extension classes");
			public static readonly GUIContent SystemCameraToggle = EditorGUIUtility.TrTextContent("Systems.Camera Assets", $"Assets from Unitylity.Systems.Camera");
			public static readonly GUIContent SystemInputToggle = EditorGUIUtility.TrTextContent("Systems.Input Assets", $"Assets from Unitylity.Systems.Input");
			public static readonly GUIContent SystemInteractionToggle = EditorGUIUtility.TrTextContent("Systems.Interaction Assets", $"Assets from Unitylity.Systems.Interaction");
			public static readonly GUIContent SystemLangToggle = EditorGUIUtility.TrTextContent("Systems.Lang Assets", $"Assets from Unitylity.Systems.Lang");
			public static readonly GUIContent SystemMenusToggle = EditorGUIUtility.TrTextContent("Systems.Menus Assets", $"Assets from Unitylity.Systems.Menus");
			public static readonly GUIContent SystemPopupsToggle = EditorGUIUtility.TrTextContent("Systems.Popups Assets", $"Assets from Unitylity.Systems.Popups");
			public static readonly GUIContent SystemRenderimagesToggle = EditorGUIUtility.TrTextContent("Systems.Renderimages Assets", $"Assets from Unitylity.Systems.Renderimages");
		}

		public UnitylitySettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }

		public override void OnActivate(string searchContext, VisualElement rootElement) {
			UnitylitySettings.instance.Save();
			serializedObject = UnitylitySettings.instance.GetSerializedObject();
		}

		[SettingsProvider]
		public static SettingsProvider CreateSettingProvider() {
			return new UnitylitySettingsProvider("Project/Unitylity", SettingsScope.Project, GetSearchKeywordsFromGUIContentProperties<Styles>());
		}

		public override void OnGUI(string searchContext) {
			serializedObject.Update();

			using (HorizontalScope()) {
				GUILayout.Space(10);
				using (VerticalScope()) {
					GUILayout.Space(10);

					using (LabelWidthScope(v => v + 100)) {

						RefreshSymbols();
						VisualStateDropdown("UNITYLITY_GENERAL_{0}", Styles.GeneralComponentsToggle);
						StateDropdown("UNITYLITY_SYSTEMS_CAMERA_{0}", Styles.SystemCameraToggle);
						StateDropdown("UNITYLITY_SYSTEMS_INPUT_{0}", Styles.SystemInputToggle);
						StateDropdown("UNITYLITY_SYSTEMS_INTERACTION_{0}", Styles.SystemInteractionToggle);
						StateDropdown("UNITYLITY_SYSTEMS_LANG_{0}", Styles.SystemLangToggle);
						StateDropdown("UNITYLITY_SYSTEMS_MENUS_{0}", Styles.SystemMenusToggle);
						StateDropdown("UNITYLITY_SYSTEMS_POPUPS_{0}", Styles.SystemPopupsToggle);
						StateDropdown("UNITYLITY_SYSTEMS_RENDERIMAGES_{0}", Styles.SystemRenderimagesToggle);

					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		private List<string> symbols = null;

		private void StateDropdown(string format, GUIContent style) {

			var hiddenSymbol = String.Format(format, "HIDDEN");
			var disabledSymbol = String.Format(format, "DISABLED");

			var v = State.Enabled;
			var changed = false;

			if (symbols.Contains(hiddenSymbol)) {
				v = (State)EditorGUILayout.EnumPopup(style, State.Hidden);
				changed |= v != State.Hidden;
			} else if (symbols.Contains(disabledSymbol)) {
				v = (State)EditorGUILayout.EnumPopup(style, State.Disabled);
				changed |= v != State.Disabled;
			} else {
				v = (State)EditorGUILayout.EnumPopup(style, State.Enabled);
				changed |= v != State.Enabled;
			}

			if (changed) {
				symbols.RemoveAll(v => v == hiddenSymbol || v == disabledSymbol);
				switch (v) {
					case State.Enabled:
						break;
					case State.Hidden:
						symbols.Add(hiddenSymbol);
						break;
					case State.Disabled:
						symbols.Add(disabledSymbol);
						break;
				}
				PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, String.Join(";", symbols));
				UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
			}

		}

		private void VisualStateDropdown(string format, GUIContent style) {

			var hiddenSymbol = String.Format(format, "HIDDEN");

			var v = VisualState.Visible;
			var changed = false;

			if (symbols.Contains(hiddenSymbol)) {
				v = (VisualState)EditorGUILayout.EnumPopup(style, VisualState.Hidden);
				changed |= v != VisualState.Hidden;
			} else {
				v = (VisualState)EditorGUILayout.EnumPopup(style, VisualState.Visible);
				changed |= v != VisualState.Visible;
			}

			if (changed) {
				symbols.RemoveAll(v => v == hiddenSymbol);
				switch (v) {
					case VisualState.Visible:
						break;
					case VisualState.Hidden:
						symbols.Add(hiddenSymbol);
						break;
				}
				PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, String.Join(";", symbols));
			}

		}

		private void RefreshSymbols() {
			symbols = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Standalone).Split(';').ToList();
		}

	}

}
#endif