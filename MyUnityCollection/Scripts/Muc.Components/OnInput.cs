﻿

namespace Muc.Components {

	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_GENERAL_COMPONENTS || !ENABLE_LEGACY_INPUT_MANAGER)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("MyUnityCollection/General/" + nameof(OnInput))]
#endif
	/// <summary>
	/// Create inputs that fire UnityEvents
	/// </summary>
	public class OnInput : MonoBehaviour {

		public InputEvent[] inputEvents;

		private HashSet<InputEvent> fixedEvents = new();

		[System.Serializable]
		public class InputEvent {
			public KeyCode key;
			public InputType type = InputType.Down;
			public bool fixedUpdate;
			public UnityEvent action;
		}

		public enum InputType { Held, NotHeld, Down, Up, }

#if ENABLE_LEGACY_INPUT_MANAGER
		void Update() {
			foreach (var inputEvent in inputEvents) {
				bool activated = false;
				switch (inputEvent.type) {
					case InputType.Down:
						activated = Input.GetKeyDown(inputEvent.key);
						break;
					case InputType.Up:
						activated = Input.GetKeyUp(inputEvent.key);
						break;
					case InputType.Held:
						activated = Input.GetKey(inputEvent.key);
						break;
					case InputType.NotHeld:
						activated = !Input.GetKey(inputEvent.key);
						break;
				}
				if (activated) {
					if (inputEvent.fixedUpdate) {
						fixedEvents.Add(inputEvent);
					} else {
						inputEvent.action.Invoke();
					}
				}
			}
		}

		void FixedUpdate() {
			foreach (var fixedEvent in fixedEvents) {
				fixedEvent.action.Invoke();
			}
			fixedEvents.Clear();
		}
#endif
	}

}

#if UNITY_EDITOR && ENABLE_LEGACY_INPUT_MANAGER
namespace Muc.Components.Editor {

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using Object = UnityEngine.Object;
	using static Muc.Editor.PropertyUtil;
	using static Muc.Editor.EditorUtil;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(OnInput), true)]
	public class OnInputEditor : Editor {

		OnInput t => (OnInput)target;

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.HelpBox("This component only supports the old Input Manager. Use the dedicated Input components for the new Input System.", MessageType.Warning);
			DrawDefaultInspector();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif