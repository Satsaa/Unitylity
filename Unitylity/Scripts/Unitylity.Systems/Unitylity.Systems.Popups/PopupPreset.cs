
namespace Unitylity.Systems.Popups {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.SceneManagement;
	using UnityEngine.Serialization;
	using UnityEngine.UI;
	using Unitylity.Data;
	using Unitylity.Systems.Lang;
	using Object = UnityEngine.Object;

#if !UNITYLITY_HIDE_SYSTEM_POPUPS
	[CreateAssetMenu(fileName = nameof(PopupPreset), menuName = "Unitylity/" + nameof(Unitylity.Systems.Popups) + "/" + nameof(PopupPreset))]
#endif
	public class PopupPreset : ScriptableObject {

		[SerializeField] internal Popup popupPrefab;
		[SerializeField] internal PopupOption optionPrefab;

		[SerializeField, Tooltip("The name of the popup in PascalCase. This string will be used to find default strings.")]
		string popupName = "MyPopup";
		string titleStrId => $"Popup_{popupName}_Title";
		string msgStrId => $"Popup_{popupName}_Message";

		public struct Option {
			public string text;
			public Action action;
			public PopupOption.Flags key;

			public Option(string text, Action action, PopupOption.Flags key = 0) {
				this.text = text;
				this.action = action;
				this.key = key;
			}
		}

		protected virtual void DoTitle(Popup msgBox, string title) {
			msgBox.SetTitle(title);
		}

		protected virtual void DoMessage(Popup msgBox, string message) {
			msgBox.SetMessage(message);
		}

		protected virtual void DoOption(Popup msgBox, Option option) {
			var opt = msgBox.AddOption(optionPrefab, option.action);
			opt.SetText(option.text);
			opt.flags = option.key;
		}

		protected virtual void DoCustom(Popup msgBox) {
			// Override if needed
		}

		public Popup Show() => Show(Lang.GetStr(titleStrId), Lang.GetStr(msgStrId), new Option(Lang.GetStr("Ok"), null, PopupOption.Flags.Cancel | PopupOption.Flags.Default));
		public Popup Show(string title) => Show(null, null, new Option(Lang.GetStr("Ok"), null, PopupOption.Flags.Cancel | PopupOption.Flags.Default));

		public Popup Show(string title, string message) => Show(null, message, new Option(Lang.GetStr("Ok"), null, PopupOption.Flags.Cancel | PopupOption.Flags.Default));
		public Popup Show(string title, string message, params Option[] options) {
			EventSystem.current.SetSelectedGameObject(null);
			if (popupPrefab == null || optionPrefab == null) {
				Debug.LogError($"{nameof(popupPrefab)} or {nameof(optionPrefab)} is not set. Alternatively a reference may be broken and you need to reassign them in editor. To do that double click this message and press the reassign button.", this);
				if (popupPrefab == null) throw new ArgumentNullException("Argument cannot be null.", nameof(popupPrefab));
				if (optionPrefab == null) throw new ArgumentNullException("Argument cannot be null.", nameof(optionPrefab));
			}
			var msgBox = Instantiate(popupPrefab);
			msgBox.gameObject.transform.SetParent(Popups.instance.rectTransform);
			DoTitle(msgBox, title);
			DoMessage(msgBox, message);
			DoCustom(msgBox);
			foreach (var option in options) {
				DoOption(msgBox, option);
			}
			return msgBox;
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Systems.Popups.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(PopupPreset), true)]
	public class PopupPresetEditor : Editor {

		PopupPreset t => (PopupPreset)target;

		SerializedProperty popupPrefab;
		SerializedProperty optionPrefab;

		void OnEnable() {
			popupPrefab = serializedObject.FindProperty(nameof(PopupPreset.popupPrefab));
			optionPrefab = serializedObject.FindProperty(nameof(PopupPreset.optionPrefab));
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			using (DisabledScope(Application.isPlaying)) {
				if (GUILayout.Button("Trigger recompile (may fix missing ref exception)")) {
					UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
				}
			}

		}
	}

}
#endif