
// License: MIT
// Original Author: JohannesMP (2017)
// Original Author: S. Tarık Çetin (2019)
// Original Code: https://github.com/JohannesMP/unity-scene-reference

namespace Unitylity.Data {

	using System;
	using System.Linq;
	using UnityEngine;
	using Object = UnityEngine.Object;
	using UnityEngine.SceneManagement;
#if UNITY_EDITOR
	using UnityEditor;
	using UnityEditor.SceneManagement;
#endif

	/// <summary>
	/// A wrapper that provides the means to safely serialize Scene Asset References.
	/// </summary>
	[Serializable]
	public class SceneReference
#if UNITY_EDITOR
		: ISerializationCallbackReceiver
#endif
	{
#if UNITY_EDITOR
		[SerializeField] internal Object sceneAsset;
		private bool isValidSceneAsset => sceneAsset && sceneAsset is SceneAsset;
#endif

		[SerializeField] internal string _scenePath = string.Empty;

		// Use this when you want to actually have the scene path
		public string scenePath {
			get {
#if UNITY_EDITOR
				// In editor we always use the asset's path
				return GetScenePathFromAsset();
#else
				// At runtime we rely on the stored path value which we assume was serialized correctly at build time.
				// See OnBeforeSerialize and OnAfterDeserialize
				return _scenePath;
#endif
			}
			set {
				_scenePath = value;
#if UNITY_EDITOR
				sceneAsset = GetSceneAssetFromPath();
#endif
			}
		}

		public Scene scene => SceneManager.GetSceneByPath(scenePath);

		public static implicit operator string(SceneReference sceneReference) {
			return sceneReference.scenePath;
		}

		// Called to prepare this data for serialization. Stubbed out when not in editor.
		public void OnBeforeSerialize() {
#if UNITY_EDITOR
			DoBeforeSerialize();
#endif
		}

		// Called to set up data for deserialization. Stubbed out when not in editor.
		public void OnAfterDeserialize() {
#if UNITY_EDITOR
			// We sadly cannot touch assetdatabase during serialization, so defer by a bit.
			EditorApplication.update += DoAfterDeserialize;
#endif
		}



#if UNITY_EDITOR
		private SceneAsset GetSceneAssetFromPath() => string.IsNullOrEmpty(_scenePath) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(_scenePath);
		private string GetScenePathFromAsset() => sceneAsset == null ? string.Empty : AssetDatabase.GetAssetPath(sceneAsset);

		private void DoBeforeSerialize() {
			// Asset is invalid but have Path to try and recover from
			if (isValidSceneAsset == false && string.IsNullOrEmpty(_scenePath) == false) {
				sceneAsset = GetSceneAssetFromPath();
				if (sceneAsset == null) _scenePath = string.Empty;

				EditorSceneManager.MarkAllScenesDirty();
			}
			// Asset takes precendence and overwrites Path
			else {
				_scenePath = GetScenePathFromAsset();
			}
		}

		private void DoAfterDeserialize() {
			EditorApplication.update -= DoAfterDeserialize;
			// Asset is valid, don't do anything - Path will always be set based on it when it matters
			if (isValidSceneAsset) return;

			// Asset is invalid but have path to try and recover from
			if (string.IsNullOrEmpty(_scenePath)) return;

			sceneAsset = GetSceneAssetFromPath();
			// No asset found, path was invalid. Make sure we don't carry over the old invalid path
			if (!sceneAsset) _scenePath = string.Empty;

			if (!Application.isPlaying) EditorSceneManager.MarkAllScenesDirty();
		}
#endif
	}

}


#if UNITY_EDITOR
namespace Unitylity.Data.Editor {

	using System;
	using System.Linq;
	using UnityEditor;
	using UnityEditor.SceneManagement;
	using UnityEditor.VersionControl;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	/// <summary>
	/// Display a Scene Reference object in the editor.
	/// If scene is valid, provides basic buttons to interact with the scene's role in Build Settings.
	/// </summary>
	[CustomPropertyDrawer(typeof(SceneReference))]
	public class SceneReferencePropertyDrawer : PropertyDrawer {

		protected const float FOOTER_HEIGHT = 24;

		/// <summary> Drawing the 'SceneReference' property </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			using (PropertyScope(position, label, property, out label)) {

				var lineRect = position;
				lineRect.height = lineHeight;

				// Draw the main Object field
				var sceneAssetProperty = GetSceneAssetProperty(property);
				EditorGUI.BeginChangeCheck();
				sceneAssetProperty.objectReferenceValue = EditorGUI.ObjectField(lineRect, label, sceneAssetProperty.objectReferenceValue, typeof(SceneAsset), false);

				var buildScene = BuildUtils.GetBuildScene(sceneAssetProperty.objectReferenceValue);
				if (EditorGUI.EndChangeCheck()) {
					if (buildScene.scene == null) GetScenePathProperty(property).stringValue = string.Empty;
				}

				if (property.isExpanded = EditorGUI.Foldout(lineRect, property.isExpanded, GUIContent.none, true)) {

					// Draw the Box Background
					var footerRect = position;
					footerRect.yMin += lineHeight + spacing;
					footerRect.height = FOOTER_HEIGHT;
					GUI.Box(EditorGUI.IndentedRect(footerRect), GUIContent.none, EditorStyles.helpBox);
					var itemsRect = EditorStyles.helpBox.padding.Remove(footerRect);

					var sceneControlID = GUIUtility.GetControlID(FocusType.Passive);
					if (!buildScene.assetGUID.Empty()) {
						// Draw the Build Settings Info of the selected Scene
						DrawSceneInfoGUI(itemsRect, buildScene, sceneControlID + 1);
					}

				} else {
					GetBuildGuiContent(buildScene, out var icon, out var buildLabel);
					var iconRect = LabelRect(position);
					iconRect.x = iconRect.x + iconRect.width - icon.image.width - indentLevel * indent;
					iconRect.width = Math.Max(32, icon.image.width) + spacing;
					EditorGUI.LabelField(iconRect, icon);
				}
			}
		}

		/// <summary> Ensure that what we draw in OnGUI always has the room it needs </summary>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			if (!property.isExpanded) return base.GetPropertyHeight(property, label);
			return lineHeight + spacing + FOOTER_HEIGHT;
		}

		/// <summary> Draws info box of the provided scene </summary>
		private void DrawSceneInfoGUI(Rect position, BuildUtils.BuildScene buildScene, int sceneControlID) {
			var disabled = BuildUtils.IsDisabled();
			var disabledWarning = disabled ? "\n\nWARNING: Build Settings is not checked out and so cannot be modified." : "";

			GetBuildGuiContent(buildScene, out var icon, out var label);

			// Left status label
			using (DisabledScope(disabled)) {
				var labelRect = DrawUtils.GetLabelRect(position);
				var iconRect = labelRect;
				iconRect.width = icon.image.width + spacing;
				labelRect.width -= iconRect.width;
				labelRect.x += iconRect.width;
				EditorGUI.PrefixLabel(iconRect, sceneControlID, icon);
				EditorGUI.PrefixLabel(labelRect, sceneControlID, label);
			}

			// Right context buttons
			var buttonRect = DrawUtils.GetFieldRect(position);
			buttonRect.width = (buttonRect.width) / 3;

			var tooltipMsg = "";
			using (DisabledScope(disabled)) {
				// NOT in build settings
				if (buildScene.buildIndex == -1) {
					buttonRect.width *= 2;
					var addIndex = EditorBuildSettings.scenes.Length;
					tooltipMsg = "Add this scene to build settings. It will be appended to the end of the build scenes as buildIndex: " + addIndex + "." + disabledWarning;
					if (DrawUtils.ButtonHelper(buttonRect, "Add...", "Add (buildIndex " + addIndex + ")", EditorStyles.miniButtonLeft, tooltipMsg))
						BuildUtils.AddBuildScene(buildScene);
					buttonRect.width /= 2;
					buttonRect.x += buttonRect.width;
				}
				// In build settings
				else {
					var isEnabled = buildScene.scene.enabled;
					var stateString = isEnabled ? "Disable" : "Enable";
					tooltipMsg = stateString + " this scene in build settings.\n" + (isEnabled ? "It will no longer be included in builds" : "It will be included in builds") + "." + disabledWarning;

					if (DrawUtils.ButtonHelper(buttonRect, stateString, stateString + " In Build", EditorStyles.miniButtonLeft, tooltipMsg))
						BuildUtils.SetBuildSceneState(buildScene, !isEnabled);
					buttonRect.x += buttonRect.width;

					tooltipMsg = "Completely remove this scene from build settings.\nYou will need to add it again for it to be included in builds!" + disabledWarning;
					if (DrawUtils.ButtonHelper(buttonRect, "Remove...", "Remove from Build", EditorStyles.miniButtonMid, tooltipMsg))
						BuildUtils.RemoveBuildScene(buildScene);
				}
			}

			buttonRect.x += buttonRect.width;

			tooltipMsg = "Open the 'Build Settings' Window for managing scenes." + disabledWarning;
			if (DrawUtils.ButtonHelper(buttonRect, "Settings", "Build Settings", EditorStyles.miniButtonRight, tooltipMsg)) {
				BuildUtils.OpenBuildSettings();
			}

		}

		private static void GetBuildGuiContent(BuildUtils.BuildScene buildScene, out GUIContent icon, out GUIContent label) {

			icon = new GUIContent();
			label = new GUIContent();

			// Missing from build scenes
			if (buildScene.buildIndex == -1) {
				icon = EditorGUIUtility.IconContent("console.erroricon.sml");
				label.text = "Not in build";
				label.tooltip = "This scene is not in the build.";
			}
			// In build scenes and enabled
			else if (buildScene.scene.enabled) {
				icon = EditorGUIUtility.IconContent("console.infoicon.sml");
				label.text = "Build index: " + buildScene.buildIndex;
				label.tooltip = "This scene is in the build.";
			}
			// In build scenes but disabled
			else {
				icon = EditorGUIUtility.IconContent("console.warnicon.sml");
				label.text = "Build index: " + buildScene.buildIndex;
				label.tooltip = "This scene is in the build but is DISABLED!";
			}
			return;
		}

		private static SerializedProperty GetSceneAssetProperty(SerializedProperty property) {
			return property.FindPropertyRelative(nameof(SceneReference.sceneAsset));
		}

		private static SerializedProperty GetScenePathProperty(SerializedProperty property) {
			return property.FindPropertyRelative(nameof(SceneReference._scenePath));
		}

		private static class DrawUtils {
			/// <summary> Draw a GUI button, choosing between a short and a long button text based on if it fits </summary>
			public static bool ButtonHelper(Rect position, string msgShort, string msgLong, GUIStyle style, string tooltip = null) {
				var content = new GUIContent(msgLong, tooltip);

				var longWidth = style.CalcSize(content).x;
				if (longWidth > position.width) content.text = msgShort;

				return GUI.Button(position, content, style);
			}

			/// <summary> Given a position rect, get its field portion </summary>
			public static Rect GetFieldRect(Rect position) {
				position.width -= EditorGUIUtility.labelWidth;
				position.x += EditorGUIUtility.labelWidth;
				return position;
			}
			/// <summary> Given a position rect, get its label portion </summary>
			public static Rect GetLabelRect(Rect position) {
				position.width = EditorGUIUtility.labelWidth - spacing;
				return position;
			}
		}

		/// <summary> Various BuildSettings interactions </summary>
		private static class BuildUtils {
			// time in seconds that we have to wait before we query again when IsDisabled() is called.
			public static float minCheckWait = 3;
			private static float lastTimeChecked;
			private static bool cachedReadonlyVal = true;

			/// <summary>
			/// A small container for tracking scene data BuildSettings
			/// </summary>
			public struct BuildScene {
				public int buildIndex;
				public GUID assetGUID;
				public string assetPath;
				public EditorBuildSettingsScene scene;
			}

			/// <summary>
			/// Check if the build settings asset is readonly.
			/// Caches value and only queries state a max of every 'minCheckWait' seconds.
			/// </summary>
			public static bool IsDisabled() {
				var curTime = Time.realtimeSinceStartup;
				var timeSinceLastCheck = curTime - lastTimeChecked;

				if (timeSinceLastCheck <= minCheckWait) return cachedReadonlyVal;

				lastTimeChecked = curTime;
				cachedReadonlyVal = QueryBuildSettingsStatus();

				return cachedReadonlyVal;
			}

			/// <summary>
			/// A blocking call to the Version Control system to see if the build settings asset is readonly.
			/// Use BuildSettingsIsDisabled for version that caches the value for better responsivenes.
			/// </summary>
			private static bool QueryBuildSettingsStatus() {
				// If no version control provider, assume not readonly
				if (!Provider.enabled) return false;

				// If we cannot checkout, then assume we are not readonly
				if (!Provider.hasCheckoutSupport) return false;

				//// If offline (and are using a version control provider that requires checkout) we cannot edit.
				//if (UnityEditor.VersionControl.Provider.onlineState == UnityEditor.VersionControl.OnlineState.Offline)
				//    return true;

				// Try to get status for file
				var status = Provider.Status("ProjectSettings/EditorBuildSettings.asset", false);
				status.Wait();

				// If no status listed we can edit
				if (status.assetList == null || status.assetList.Count != 1) return true;

				// If is checked out, we can edit
				return !status.assetList[0].IsState(Asset.States.CheckedOutLocal);
			}

			/// <summary> For a given Scene Asset object reference, extract its build settings data, including buildIndex. </summary>
			public static BuildScene GetBuildScene(Object sceneObject) {

				var entry = new BuildScene {
					buildIndex = -1,
					assetGUID = new GUID(string.Empty)
				};

				if (sceneObject as SceneAsset == null) return entry;

				entry.assetPath = AssetDatabase.GetAssetPath(sceneObject);
				entry.assetGUID = new GUID(AssetDatabase.AssetPathToGUID(entry.assetPath));

				var scenes = EditorBuildSettings.scenes;
				for (var index = 0; index < scenes.Length; ++index) {
					if (!entry.assetGUID.Equals(scenes[index].guid)) continue;

					entry.scene = scenes[index];
					entry.buildIndex = index;
					return entry;
				}

				return entry;
			}

			/// <summary> Enable/Disable a given scene in the buildSettings </summary>
			public static void SetBuildSceneState(BuildScene buildScene, bool enabled) {
				var modified = false;
				var scenesToModify = EditorBuildSettings.scenes;
				foreach (var curScene in scenesToModify.Where(curScene => curScene.guid.Equals(buildScene.assetGUID))) {
					curScene.enabled = enabled;
					modified = true;
					break;
				}
				if (modified) EditorBuildSettings.scenes = scenesToModify;
			}

			/// <summary> Display Dialog to add a scene to build settings </summary>
			public static void AddBuildScene(BuildScene buildScene, bool force = false, bool enabled = true) {
				if (force == false) {
					var selection = EditorUtility.DisplayDialogComplex(
						"Add Scene To Build",
						"You are about to add scene at " + buildScene.assetPath + " To the Build Settings.",
						"Add as Enabled",  // option 0
						"Add as Disabled", // option 1
						"Cancel"           // option 2
					);

					switch (selection) {
						case 0: // enabled
							enabled = true;
							break;
						case 1: // disabled
							enabled = false;
							break;
						default:
							//case 2: // cancel
							return;
					}
				}

				var newScene = new EditorBuildSettingsScene(buildScene.assetGUID, enabled);
				var tempScenes = EditorBuildSettings.scenes.ToList();
				tempScenes.Add(newScene);
				EditorBuildSettings.scenes = tempScenes.ToArray();
			}

			/// <summary> Display Dialog to remove a scene from build settings (or just disable it) </summary>
			public static void RemoveBuildScene(BuildScene buildScene) {

				if (!EditorUtility.DisplayDialog(
					"Remove Scene From Build",
					$"You are about to remove the following scene from build settings:\n{buildScene.assetPath}\nBuild index:{buildScene.buildIndex}\n\nThis will modify build settings, but the scene asset will remain untouched.",
					"Remove From Build",
					"Cancel"
				)) {
					return;
				}

				// User chose to fully remove the scene from build settings
				else {
					var tempScenes = EditorBuildSettings.scenes.ToList();
					tempScenes.RemoveAll(scene => scene.guid.Equals(buildScene.assetGUID));
					EditorBuildSettings.scenes = tempScenes.ToArray();
				}
			}

			/// <summary> Open the default Unity Build Settings window </summary>
			public static void OpenBuildSettings() {
				EditorWindow.GetWindow(typeof(BuildPlayerWindow));
			}
		}

	}

}
#endif