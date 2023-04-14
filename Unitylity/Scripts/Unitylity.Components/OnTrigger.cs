
namespace Unitylity.Components {

	using UnityEngine;
	using UnityEngine.Events;

#if UNITYLITY_GENERAL_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/General/" + nameof(OnTrigger))]
#endif
	public class OnTrigger : MonoBehaviour {

		public bool groupColliders;

		public bool useTag;
		public string _tag = "Untagged";

		public bool useLayers;
		public LayerMask layers = ~0;

		public OnTriggerEvent onEnter;
		public OnTriggerEvent onStay;
		public OnTriggerEvent onExit;

		[System.Serializable]
		public class OnTriggerEvent : UnityEvent<Collider> { }

		private int count;

		// Start is called before the first frame update
		void Start() {
		}

		void OnTriggerEnter(Collider other) {
			if (!useTag || other.tag == _tag) {
				if (!useLayers || layers == (layers | (1 << gameObject.layer))) {
					if (groupColliders) {
						count++;
						if (count <= 1) {
							count = 1;
							onEnter.Invoke(other);
						}
					} else {
						onEnter.Invoke(other);
					}
				}
			}
		}
		void OnTriggerExit(Collider other) {
			if (!useTag || other.tag == _tag) {
				if (!useLayers || layers == (layers | (1 << gameObject.layer))) {
					onStay.Invoke(other);
				}
			}
		}
		void OnTriggerStay(Collider other) {
			if (!useTag || other.tag == _tag) {
				if (!useLayers || layers == (layers | (1 << gameObject.layer))) {
					if (groupColliders) {
						count--;
						if (count <= 0) {
							count = 0;
							onExit.Invoke(other);
						}
					} else {
						onExit.Invoke(other);
					}
				}
			}
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Components.Editor {

	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(OnTrigger))]
	public class OnTriggerEditor : Editor {

		private OnTrigger t { get => (OnTrigger)target; }

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
			groupColliders = serializedObject.FindProperty(nameof(OnTrigger.groupColliders));

			useTag = serializedObject.FindProperty(nameof(OnTrigger.useTag));
			_tag = serializedObject.FindProperty(nameof(OnTrigger._tag));
			useLayers = serializedObject.FindProperty(nameof(OnTrigger.useLayers));
			layers = serializedObject.FindProperty(nameof(OnTrigger.layers));

			onEnter = serializedObject.FindProperty(nameof(OnTrigger.onEnter));
			onStay = serializedObject.FindProperty(nameof(OnTrigger.onStay));
			onExit = serializedObject.FindProperty(nameof(OnTrigger.onExit));

			addMenuIcon = EditorGUIUtility.TrIconContent("Toolbar Plus More", "Choose to add to list");
			addMenu = new GenericMenu();

			var types = Assembly.GetAssembly(typeof(Collider)).GetTypes().Where(type => type.IsSubclassOf(typeof(Collider)));
			foreach (var type in types) {
				addMenu.AddItem(new GUIContent(type.Name), false, () => ((Collider)Undo.AddComponent(t.gameObject, type)).isTrigger = true);
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
			} else if (cols.All(c => !c.isTrigger)) {
				EditorGUILayout.HelpBox("This GameObject has no Collider that is a trigger!", MessageType.Warning);
			}

			using (new GUILayout.HorizontalScope()) {
				EditorGUILayout.PropertyField(groupColliders, new GUIContent(), GUILayout.MaxWidth(18));
				EditorGUILayout.LabelField(
					new GUIContent(
						"First In, Last Out",
						"When enabled: \n" +
						$"Only the first entering Collider invokes {onEnter.displayName}. \n" +
						$"Only the last exiting Collider invokes {onExit.displayName}."
					)
				);
			}

			using (new GUILayout.HorizontalScope()) {
				EditorGUILayout.PropertyField(useTag, new GUIContent(), GUILayout.MaxWidth(18));
				_tag.stringValue = EditorGUILayout.TagField(new GUIContent("Filter by Tag", "Require Colliders to have this tag"), _tag.stringValue);
			}

			using (new GUILayout.HorizontalScope()) {
				EditorGUILayout.PropertyField(useLayers, new GUIContent(), GUILayout.MaxWidth(18));
				EditorGUILayout.PropertyField(layers, new GUIContent("Filter by Layer", "Require Colliders' layers to be on this layer mask"));
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