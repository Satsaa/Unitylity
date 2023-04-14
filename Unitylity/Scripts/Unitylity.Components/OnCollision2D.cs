
namespace Unitylity.Components {

	using UnityEngine;
	using UnityEngine.Events;

#if UNITYLITY_GENERAL_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/General/" + nameof(OnCollision2D))]
#endif
	public class OnCollision2D : MonoBehaviour {

		public bool groupColliders;

		public bool useTag;
		public string _tag = "Untagged";

		public bool useLayers;
		public LayerMask layers = ~0;

		public OnCollision2DEvent onEnter;
		public OnCollision2DEvent onStay;
		public OnCollision2DEvent onExit;

		[System.Serializable]
		public class OnCollision2DEvent : UnityEvent<Collision2D> { }

		private int count;

		// Start is called before the first frame update
		void Start() {
		}

		void OnCollisionEnter2D(Collision2D collision) {
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
		void OnCollisionExit2D(Collision2D collision) {
			if (!useTag || collision.gameObject.tag == _tag) {
				if (!useLayers || layers == (layers | (1 << gameObject.layer))) {
					onStay.Invoke(collision);
				}
			}
		}
		void OnCollisionStay2D(Collision2D collision) {
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
namespace Unitylity.Components.Editor {

	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(OnCollision2D))]
	public class OnCollision2DEditor : Editor {

		private OnCollision2D t { get => (OnCollision2D)target; }

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
			groupColliders = serializedObject.FindProperty(nameof(OnCollision2D.groupColliders));

			useTag = serializedObject.FindProperty(nameof(OnCollision2D.useTag));
			_tag = serializedObject.FindProperty(nameof(OnCollision2D._tag));
			useLayers = serializedObject.FindProperty(nameof(OnCollision2D.useLayers));
			layers = serializedObject.FindProperty(nameof(OnCollision2D.layers));

			onEnter = serializedObject.FindProperty(nameof(OnCollision2D.onEnter));
			onStay = serializedObject.FindProperty(nameof(OnCollision2D.onStay));
			onExit = serializedObject.FindProperty(nameof(OnCollision2D.onExit));

			addMenuIcon = EditorGUIUtility.TrIconContent("Toolbar Plus More", "Choose to add to list");
			addMenu = new GenericMenu();

			var types = Assembly.GetAssembly(typeof(Collider2D)).GetTypes().Where(type => type.IsSubclassOf(typeof(Collider2D)));
			foreach (var type in types) {
				addMenu.AddItem(new GUIContent(type.Name), false, () => Undo.AddComponent(t.gameObject, type));
			}
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			var cols = t.GetComponents<Collider2D>();
			if (cols.Length == 0) {
				using (new GUILayout.HorizontalScope()) {
					EditorGUILayout.HelpBox("This GameObject has no Collider2D!", MessageType.Warning);
					if (GUILayout.Button(addMenuIcon, GUILayout.ExpandHeight(true))) {
						addMenu.ShowAsContext();
					}
				}
			} else if (cols.All(c => c.isTrigger)) {
				EditorGUILayout.HelpBox("This GameObject has no Collider2D that is not a trigger!", MessageType.Warning);
			}

			using (new GUILayout.HorizontalScope()) {
				EditorGUILayout.PropertyField(groupColliders, new GUIContent(), GUILayout.MaxWidth(18));
				EditorGUILayout.LabelField(
					new GUIContent(
						"First In, Last Out",
						"When enabled: \n" +
						$"Only the first colliding collider invokes {onEnter.displayName}. \n" +
						$"Only the last uncolliding collider invokes {onExit.displayName}."
					)
				);
			}

			using (new GUILayout.HorizontalScope()) {
				EditorGUILayout.PropertyField(useTag, new GUIContent(), GUILayout.MaxWidth(18));
				_tag.stringValue = EditorGUILayout.TagField(new GUIContent("Filter by Tag", "Require colliders to have this tag"), _tag.stringValue);
			}

			using (new GUILayout.HorizontalScope()) {
				EditorGUILayout.PropertyField(useLayers, new GUIContent(), GUILayout.MaxWidth(18));
				EditorGUILayout.PropertyField(layers, new GUIContent("Filter by Layer", "Require colliders layer to be on this layer mask"));
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