﻿

namespace Muc.Components {

	using UnityEngine;
	using UnityEngine.Events;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_GENERAL_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("MyUnityCollection/General/" + nameof(OnCollision))]
#endif
	public class OnCollision : MonoBehaviour {

		public bool groupColliders;

		public bool useTag;
		public string _tag = "Untagged";

		public bool useLayers;
		public LayerMask layers = ~0;

		public OnCollisionEvent onEnter;
		public OnCollisionEvent onStay;
		public OnCollisionEvent onExit;

		[System.Serializable]
		public class OnCollisionEvent : UnityEvent<Collision> { }

		private int count;

		// Start is called before the first frame update
		void Start() {
		}

		void OnCollisionEnter(Collision collision) {
			if (!useTag || collision.gameObject.tag == _tag) {
				if (!useLayers || layers == (layers | (1 << gameObject.layer))) {
					if (groupColliders) {
						count++;
						if (count <= 1) {
							count = 1;
							onEnter.Invoke(collision);
						}
					} else {
						onEnter.Invoke(collision);
					}
				}
			}
		}
		void OnCollisionExit(Collision collision) {
			if (!useTag || collision.gameObject.tag == _tag) {
				if (!useLayers || layers == (layers | (1 << gameObject.layer))) {
					onStay.Invoke(collision);
				}
			}
		}
		void OnCollisionStay(Collision collision) {
			if (!useTag || collision.gameObject.tag == _tag) {
				if (!useLayers || layers == (layers | (1 << gameObject.layer))) {
					if (groupColliders) {
						count--;
						if (count <= 0) {
							count = 0;
							onExit.Invoke(collision);
						}
					} else {
						onExit.Invoke(collision);
					}
				}
			}
		}
	}

}


#if UNITY_EDITOR
namespace Muc.Components.Editor {

	using System.Reflection;
	using System.Linq;

	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(OnCollision))]
	public class OnCollisionEditor : Editor {

		private OnCollision t { get => (OnCollision)target; }

		private SerializedProperty groupColliders;

		private SerializedProperty useTag;
		private SerializedProperty _tag;
		private SerializedProperty useLayers;
		private SerializedProperty layers;

		private SerializedProperty onEnter;
		private SerializedProperty onStay;
		private SerializedProperty onExit;

		private GUIContent addMenuIcon;
		private GenericMenu addMenu;

		void OnEnable() {
			groupColliders = serializedObject.FindProperty(nameof(OnCollision.groupColliders));

			useTag = serializedObject.FindProperty(nameof(OnCollision.useTag));
			_tag = serializedObject.FindProperty(nameof(OnCollision._tag));
			useLayers = serializedObject.FindProperty(nameof(OnCollision.useLayers));
			layers = serializedObject.FindProperty(nameof(OnCollision.layers));

			onEnter = serializedObject.FindProperty(nameof(OnCollision.onEnter));
			onStay = serializedObject.FindProperty(nameof(OnCollision.onStay));
			onExit = serializedObject.FindProperty(nameof(OnCollision.onExit));

			addMenuIcon = EditorGUIUtility.TrIconContent("Toolbar Plus More", "Choose to add to list");
			addMenu = new GenericMenu();

			var types = Assembly.GetAssembly(typeof(Collider)).GetTypes().Where(type => type.IsSubclassOf(typeof(Collider)));
			foreach (var type in types) {
				addMenu.AddItem(new GUIContent(type.Name), false, () => Undo.AddComponent(t.gameObject, type));
			}
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			var cols = t.GetComponents<Collider>();
			if (cols.Length == 0) {
				using (new GUILayout.HorizontalScope()) {
					EditorGUILayout.HelpBox("This GameObject has no Collider!", MessageType.Warning);
					if (GUILayout.Button(addMenuIcon, GUILayout.ExpandHeight(true))) {
						addMenu.ShowAsContext();
					}
				}
			} else if (cols.All(c => c.isTrigger)) {
				EditorGUILayout.HelpBox("This GameObject has no Collider that is not a trigger!", MessageType.Warning);
			}

			using (new GUILayout.HorizontalScope()) {
				EditorGUILayout.PropertyField(groupColliders, new GUIContent(), GUILayout.MaxWidth(18));
				EditorGUILayout.LabelField(
					new GUIContent(
						"First In, Last Out",
						"When enabled: \n" +
						$"Only the first colliding Collider invokes {onEnter.displayName}. \n" +
						$"Only the last uncolliding Collider invokes {onExit.displayName}."
					)
				);
			}

			using (new GUILayout.HorizontalScope()) {
				EditorGUILayout.PropertyField(useTag, new GUIContent(), GUILayout.MaxWidth(18));
				_tag.stringValue = EditorGUILayout.TagField(new GUIContent("Filter by Tag", "Require Colliders to have this tag"), _tag.stringValue);
			}

			using (new GUILayout.HorizontalScope()) {
				EditorGUILayout.PropertyField(useLayers, new GUIContent(), GUILayout.MaxWidth(18));
				EditorGUILayout.PropertyField(layers, new GUIContent("Filter by Layer", "Require Colliders layer to be on this layer mask"));
			}

			EditorGUILayout.Separator();

			EditorGUILayout.PropertyField(onEnter);
			EditorGUILayout.PropertyField(onStay);
			EditorGUILayout.PropertyField(onExit);

			serializedObject.ApplyModifiedProperties();
		}
	}

}
#endif