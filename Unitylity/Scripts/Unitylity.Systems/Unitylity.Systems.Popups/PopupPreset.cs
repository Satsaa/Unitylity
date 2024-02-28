
namespace Unitylity.Systems.Popups {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using TMPro;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.SceneManagement;
	using UnityEngine.Serialization;
	using UnityEngine.UI;
	using Unitylity.Data;
	using Object = UnityEngine.Object;

#if !UNITYLITY_HIDE_SYSTEM_POPUPS
	[CreateAssetMenu(fileName = nameof(PopupPreset), menuName = "Unitylity/" + nameof(Unitylity.Systems.Popups) + "/" + nameof(PopupPreset))]
#endif
	public class PopupPreset : ScriptableObject {

		[SerializeField] internal Popup popupPrefab;
		[SerializeField] internal PopupOption optionPrefab;
		[SerializeField] string title;
		[SerializeField] string message;
		[SerializeField] internal Option[] defaultOptions;

		protected static Option[] ok = new Option[1] { new("Ok", null, PopupOption.Flags.Cancel | PopupOption.Flags.Default) };
		protected Option[] defaultsOrOk => defaultOptions.Any() ? defaultOptions : ok;

		[Serializable]
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

		public Popup Show() => Show(title, message, defaultsOrOk);
		public Popup Show(string title) => Show(title, message, defaultsOrOk);
		public Popup Show(string title, string message) => Show(title, message, defaultsOrOk);
		public Popup Show(params Option[] options) => Show(title, message, options);
		public Popup Show(string title, string message, params Option[] options) {
			EventSystem.current.SetSelectedGameObject(null);
			if (popupPrefab == null || optionPrefab == null) {
				Debug.LogError($"{nameof(popupPrefab)} or {nameof(optionPrefab)} is not set. Alternatively a reference may be broken and you need to reassign them in editor. To do that double click this message and press the reassign button.", this);
				if (popupPrefab == null) throw new ArgumentNullException("Argument cannot be null.", nameof(popupPrefab));
				if (optionPrefab == null) throw new ArgumentNullException("Argument cannot be null.", nameof(optionPrefab));
			}
			var popup = Instantiate(popupPrefab, Popups.instance.rectTransform);
			popup.defaultOptionPrefab = optionPrefab;
			DoTitle(popup, title);
			DoMessage(popup, message);
			DoCustom(popup);
			foreach (var option in options) {
				DoOption(popup, option);
			}
			if (popup.TryGetComponent<Animator>(out var animator)) {
				animator.SetTrigger("Show");
			}
			return popup;
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