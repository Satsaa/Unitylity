
namespace Unitylity.Systems.Interaction {

	using UnityEngine;
	using Unitylity.Components;

#if UNITYLITY_SYSTEMS_INTERACTION_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Interaction) + "/" + nameof(Interactor))]
#endif
	public class Interactor : MonoBehaviour {

		[Tooltip("The maximum distance to " + nameof(Interactable) + "'s collider. (Calculated with nearest point)")]
		public float maxDistance = 2;

		[Tooltip("The maximum distance to the hit point of the raycast")]
		public float rayLength = 3f;

		[Tooltip("Layer mask used for raycast")]
		public LayerMask mask;

		[Tooltip("When interacting with a dynamic/movable " + nameof(Interactable) + " ignore collisions of this collider and the target collider")]
		public Collider associatedCollider;

		[Tooltip(
			nameof(Type.Hold) + ": Activate on press and deactivate on release.\n\n" +
			nameof(Type.Toggle) + ": Activate on press and deactivate when pressed again.\n\n" +
			nameof(Type.Instant) + ": Activate on press and deactivate immediately."
		)]
		public Type type = Type.Hold;
		public enum Type { Hold, Toggle, Instant }


		public DeactivationRules restrictions;
		[System.Serializable]
		public class DeactivationRules {

			[Tooltip("Deactivate if the raycast no longer hits the " + nameof(Interactable))]
			public bool sigth = true;
			[Tooltip("The mask used for the sight raycast")]
			public LayerMask sigthMask;

			[Tooltip("Deactivate " + nameof(Interactable) + " if the distance exceeds the value of " + nameof(maxDistance) + " multiplied by 1.1")]
			public bool distance = true;

			[Tooltip("Deactivate if the angle to " + nameof(Interactable) + " exceeds this"), Range(0, 180)]
			public float angle = 180f;
		}

		public Options prefs;
		[System.Serializable]
		public class Options {

			[Tooltip("Maximum force applied to an object (e.g. when colliding or throwing)")]
			public float maxForce = 1;
		}


		public Interactable interactable { get; private set; }
		public Interaction interaction { get; private set; }
		public TransformHistory transHistory { get; private set; }

		private Vector3 prevForward;
		private bool pressed;
		private bool pressedThisFrame;

		void Start() {
			transHistory = GetComponent<TransformHistory>() ?? gameObject.AddComponent<TransformHistory>();
		}

		void LateUpdate() {
			var pressedThisFrame = this.pressedThisFrame;
			this.pressedThisFrame = false;
			// If an interaction is happening
			if (interaction) {
				// Maybe the interaction was ended somewhere else?
				if (interaction.ended) {
					interaction = null;
					// Recall Update so new interactions can be immediately recognized
					LateUpdate();
					return;
				}
				if (type == Type.Toggle ? pressed : !pressedThisFrame || !CompliesWithRestrictions()) {
					interactable.Deactivate(out var _);
					return;
				}
				interactable.Active();
			} else {
				var pos = transform.position;
				// Check if targetting something valid and do appropriate things
				Debug.DrawRay(pos, transform.forward * rayLength);
				var prevInteractable = interactable;
				if (Physics.Raycast(pos, transform.forward, out RaycastHit hit, rayLength, mask)) {
					// If hit interactable object
					if (hit.collider.TryGetComponent<Interactable>(out var _interactable)) {
						interactable = _interactable;
						// Check if a different interactable
						if (prevInteractable && interactable != prevInteractable) {
							if (prevInteractable.targeted) {
								prevInteractable.Untarget();
							}
						}
						// Check if within the required distance
						if (hit.distance < maxDistance || Vector3.Distance(pos, hit.collider.ClosestPoint(pos)) < maxDistance) {
							if (pressed) {
								// Pressed. Activate the interactable
								switch (type) {
									case Type.Hold:
									case Type.Toggle:
										interaction = interactable.Activate(this);
										interactable.Active();
										break;
									case Type.Instant:
										interactable.Activate(this);
										interactable.Deactivate(out var _);
										break;
								}
							} else {
								// Not pressed. Target if necessary
								if (!interactable.targeted) {
									interactable.Target();
								}
							}
						}
					} else {
						// No Interactable was hit. Untarget if necessary
						if (prevInteractable && prevInteractable.targeted) {
							prevInteractable.Untarget();
						}
					}
				} else {
					// Nothing was hit. Untarget if necessary
					if (prevInteractable && prevInteractable.targeted) {
						prevInteractable.Untarget();
					}
				}
			}
			// Use these values for sight check etc. because the target is moved after the checks
			// and this object moves independently
			prevForward = transform.forward;
		}

		/// <summary>
		/// Sets the interaction to be started
		/// </summary>
		public void SetPressed(bool pressed) {
			if (this.pressed != (this.pressed = pressed)) {
				if (pressed) {
					pressedThisFrame = true;
				}
			}
		}

		bool CompliesWithRestrictions() {

			Collider targetCollider = null;
			if (restrictions.sigth) {
				if (Physics.Raycast(transHistory[1], prevForward, out var hit, float.PositiveInfinity, restrictions.sigthMask)) {
					if (hit.collider.gameObject != interaction.target.gameObject) return false;
					else targetCollider = hit.collider;
				} else {
					return false;
				}
			}

			if (restrictions.distance) {
				if (!targetCollider) targetCollider = interaction.target.GetComponent<Collider>();
				if (Vector3.Distance(transHistory[1], targetCollider.ClosestPoint(transHistory[1])) > maxDistance * 1.1f) return false;
			}

			if (restrictions.angle < 180) {
				var to = prevForward;
				var from = (interaction.targetPos - transHistory[1]).normalized;
				if (Vector3.Angle(to, from) > restrictions.angle) return false;
			}

			return true;
		}

	}

}