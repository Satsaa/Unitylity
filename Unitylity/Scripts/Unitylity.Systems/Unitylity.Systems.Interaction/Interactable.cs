
namespace Unitylity.Systems.Interaction {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

#if UNITYLITY_SYSTEMS_INTERACTION_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Interaction) + "/" + nameof(Interactable))]
#endif
	[RequireComponent(typeof(Collider))]
	public class Interactable : MonoBehaviour {

		[Tooltip("When the interactable gains focus")]
		public UnityEvent<Interactable> onTarget;
		[Tooltip("When the interactable loses focus")]
		public UnityEvent<Interactable> onUntarget;

		[Tooltip("When the interaction is started")]
		public InteractionEvent onActivate;
		[Tooltip("When the interaction is ongoing")]
		public InteractionEvent onActive;
		[Tooltip("When the interaction is ended")]
		public InteractionEvent onDeactivate;

		[System.Serializable]
		public class InteractableEvent : UnityEvent<Interactable> { };
		[System.Serializable]
		public class InteractionEvent : UnityEvent<Interaction> { };

		/// <summary> Whether this interactable is being targeted </summary>
		public bool targeted { get; protected set; }

		/// <summary> Whether this interactable is being interacted with </summary>
		public bool interacting { get => !interaction.ended; }

		/// <summary> The current interaction or null </summary>
		public Interaction interaction { get; protected set; }

		public void AddActivationEventListeners(UnityAction<Interaction> onActivate, UnityAction<Interaction> onActive, UnityAction<Interaction> onDeactivate) {
			this.onActivate.AddListener(onActivate);
			this.onActive.AddListener(onActive);
			this.onDeactivate.AddListener(onDeactivate);
		}
		public void AddActivationEventListeners(UnityAction<Interaction> onActivate, UnityAction<Interaction> onDeactivate) {
			this.onActivate.AddListener(onActivate);
			this.onDeactivate.AddListener(onDeactivate);
		}

		public void Target() {
			if (targeted) {
				Debug.LogWarning($"Trying to target an {nameof(Interactable)} multiple times");
				return;
			}
			targeted = true;
			onTarget.Invoke(this);
		}
		public void Untarget() {
			if (!targeted) {
				Debug.LogWarning($"Trying to untarget an {nameof(Interactable)} multiple times");
				return;
			}
			targeted = false;
			onUntarget.Invoke(this);
		}

		/// <summary>
		/// <para> Activates the Interactable. </para>
		/// <para> If the Interactable is targeted, it will be untargeted. </para>
		/// </summary>
		public Interaction Activate(Interactor source) {
			if (targeted) Untarget();
			if (interaction) {
				interaction.End();
				onDeactivate.Invoke(interaction);
			}
			interaction = new Interaction(source, this);
			onActivate.Invoke(interaction);
			return interaction;
		}

		/// <summary> Invokes the onActivate listeners </summary>
		public void Active() {
			onActive.Invoke(interaction);
		}

		/// <summary> Deactivates the Interactable </summary>
		/// <return> Whether or not an Interaction was ended (false if already ended or non existing) </returns>
		public bool Deactivate(out Interaction endedInteraction) {
			endedInteraction = interaction;
			if (!interaction || interaction.ended) {
				interaction = null;
				return false;
			}
			interaction.InternalEnd();
			onDeactivate.Invoke(interaction);
			interaction = null;
			return true;
		}
		/// <summary> Deactivates the Interactable without calling End on the interaction (preventing the loop) </summary>
		internal void InternalDeactivate() {
			onDeactivate.Invoke(interaction);
			interaction = null;
		}
	}

	public class Interaction {

		public readonly Interactor source;
		public readonly Interactable target;
		public readonly float startDistance;
		public readonly float startTime;

		public Vector3 sourcePos { get => source.transform.position; set => source.transform.position = value; }
		public Vector3 targetPos { get => target.transform.position; set => target.transform.position = value; }

		/// <summary> Vector between source and target </summary>
		public Vector3 dif {
			get => targetPos - sourcePos;
			set => target.transform.position = sourcePos + value;
		}

		public float distance {
			get => Vector3.Distance(sourcePos, targetPos);
			set => target.transform.position = sourcePos + dif * value;
		}

		public float duration { get => ended ? endTime - startTime : Time.time - startTime; }

		public bool ended { get; private set; }
		public float endTime { get; private set; }

		public static implicit operator bool(Interaction interaction) => interaction != null;

		public Interaction(Interactor source, Interactable target) : this(source, target, Time.time) { }
		public Interaction(Interactor source, Interactable target, float startTime) {
			this.source = source;
			this.target = target;
			this.startDistance = Vector3.Distance(source.transform.position, target.transform.position);
			this.startTime = startTime;
		}

		public void End() => End(Time.time);
		public void End(float time) {
			_End(time);
			target.InternalDeactivate();
		}
		internal void InternalEnd() => InternalEnd(Time.time);
		internal void InternalEnd(float time) {
			_End(time);
		}
		private void _End(float time) {
			if (ended) return;
			ended = true;
			endTime = time;
		}

	}

}