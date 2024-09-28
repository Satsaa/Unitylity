
namespace Unitylity.Systems.Interaction {

	using UnityEngine;

#if UNITYLITY_SYSTEMS_INTERACTION_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Interaction) + "/" + nameof(Throwable))]
#endif
	[RequireComponent(typeof(Movable))]
	public class Throwable : MonoBehaviour {

		public Movable movable { get; private set; }

		void Start() {
			movable = GetComponent<Movable>();
		}

		public void Throw() {
			if (movable.interactable.Deactivate(out var interaction)) {
				var source = interaction.source;
				movable.rb.linearVelocity = Vector3.zero;
				movable.rb.AddForce(source.transform.forward * source.prefs.maxForce, ForceMode.Impulse);
			}
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Systems.Interaction.Editor {

	using UnityEditor;

	[CustomEditor(typeof(Throwable))]
	public class ThrowableEditor : Editor {
		public override void OnInspectorGUI() {
			EditorGUILayout.LabelField("Call the function Throw of this component from another script");
		}

	}

}
#endif