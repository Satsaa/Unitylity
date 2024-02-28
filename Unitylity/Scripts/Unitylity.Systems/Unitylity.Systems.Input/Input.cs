
namespace Unitylity.Systems.Input {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using Unitylity.Extensions;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.InputSystem;

	public abstract class Input<T> : Input {

		public T value { get; protected set; }
		public UnityEvent<T> onChange;

		public void LogValue(T value) {
			Debug.Log(value);
		}
	}

	public abstract class Input : MonoBehaviour {

		[SerializeField] protected internal InputActionAsset _actionAsset;
		public InputActionAsset actionAsset {
			get => _actionAsset;
			set {
				if (_actionAsset == value) return;
				ClearCallbacks();
				_actionAsset = value;
				SetCallbacks();
			}
		}

		[SerializeField] protected internal string _actionId;
		public string actionId {
			get => _actionId;
			set {
				if (_actionId == value) return;
				ClearCallbacks();
				_actionId = value;
				SetCallbacks();
			}
		}

		protected void Reset() {
			if (!actionAsset && TryGetComponent<Input>(out var other)) {
				actionAsset = other.actionAsset;
			}
		}

		protected void OnEnable() {
			SetCallbacks();
		}

		protected void OnDisable() {
			ClearCallbacks();
		}

		/// <summary>
		/// <para>Called when the InputAction's performed, canceled, or started event is triggered.</para>
		/// <para>Update the value and call UnityEvents in this function.</para>
		/// </summary>
		protected abstract void OnInputUpdate(InputAction.CallbackContext context);

		/// <summary>
		/// Whether or not an InputControl is supported for this type of InputSource.
		/// /// </summary>
		internal abstract bool IsControlSupported(InputControl control);

		internal void ClearCallbacks() {
			// Debug.Log($"Clearing: {actionAsset && !String.IsNullOrEmpty(actionId)}");
			if (actionAsset && !String.IsNullOrEmpty(actionId)) {
				var action = actionAsset.FindAction(actionId);
				if (action != null) {
					action.performed -= OnInputUpdate;
					action.canceled -= OnInputUpdate;
					action.started -= OnInputUpdate;
					action.performed -= OnInputUpdate;
					action.canceled -= OnInputUpdate;
					action.started -= OnInputUpdate;
				}
			}
		}

		internal void SetCallbacks() {
			// Debug.Log($"Setting: {enabled && actionAsset && !String.IsNullOrEmpty(actionId)}");
			if (enabled && actionAsset && !String.IsNullOrEmpty(actionId)) {
				var action = actionAsset.FindAction(actionId);
				if (action != null) {
					action.performed -= OnInputUpdate;
					action.canceled -= OnInputUpdate;
					action.started -= OnInputUpdate;
					action.performed -= OnInputUpdate;
					action.canceled -= OnInputUpdate;
					action.started -= OnInputUpdate;
					action.performed += OnInputUpdate;
					action.canceled += OnInputUpdate;
					action.started += OnInputUpdate;
					if (Application.isPlaying)
						action.Enable();
				}
			}
		}

#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnReloadScripts() {
			foreach (var inputSource in GameObject.FindObjectsByType<Input>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
				inputSource.SetCallbacks();
			}
		}
#endif

	}

}


#if UNITY_EDITOR
namespace Unitylity.Systems.Input.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;
	using UnityEngine.InputSystem;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Input = Unitylity.Systems.Input.Input;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(Input), true)]
	public class InputSourceEditor : Editor {

		Input t => (Input)target;

		SerializedProperty _actionId;
		SerializedProperty _actionAsset;

		void OnEnable() {
			_actionId = serializedObject.FindProperty(nameof(Input._actionId));
			_actionAsset = serializedObject.FindProperty(nameof(Input._actionAsset));
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			ScriptField(serializedObject);

			EditorGUILayout.PropertyField(_actionAsset);

			if (_actionAsset.objectReferenceValue) {

				var options = new List<(int sorting, string id, string display)>() {
				(0, "", "None")
			};

				foreach (var actionMap in (_actionAsset.objectReferenceValue as InputActionAsset).actionMaps) {
					foreach (var action in actionMap.actions) {
						var supported = action.controls.Count(v => t.IsControlSupported(v));
						if (supported == 0) {
							options.Add((3, action.id.ToString(), $"{action.actionMap.name}/{action.name} (Incompatible)"));
						} else if (supported == action.controls.Count) {
							options.Add((1, action.id.ToString(), $"{action.actionMap.name}/{action.name}"));
						} else {
							options.Add((2, action.id.ToString(), $"{action.actionMap.name}/{action.name} (Partially incompatible)"));
						}
					}
				}

				options.Sort((a, b) => a.sorting - b.sorting);

				var selected = options.FindIndex(v => v.id == _actionId.stringValue);
				if (selected < 0) selected = 0;

				if (selected != (selected = EditorGUILayout.Popup("Input Action", selected, options.Select(v => $"{v.display}").ToArray()))) {
					_actionId.stringValue = options[selected].id;
				}
			}

			DrawPropertiesExcluding(serializedObject,
				script,
				_actionId.name,
				_actionAsset.name
			);

			serializedObject.ApplyModifiedProperties();
		}

	}

}
#endif