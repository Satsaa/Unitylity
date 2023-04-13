
namespace Unitylity.Systems.Camera {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Camera) + "/" + nameof(UnitylityCameraZoom))]
#endif
	[RequireComponent(typeof(UnitylityCamera))]
	public class UnitylityCameraZoom : MonoBehaviour {

		[Min(0), Tooltip("Distance multiplier.")]
		public float multiplier = 1.2f;

		[Min(0), Tooltip("Minimum change in distance.")]
		public float minStep = 0.1f;

		[Tooltip("Minimum and maximum or distance.")]
		public Vector2 range = new(1.2f, 25f);

		[Tooltip("Normalize the input to a length of 1?")]
		public bool normalize = false;


		[SerializeField, HideInInspector]
		private UnitylityCamera mucam;


		protected void Awake() {
			mucam = gameObject.GetComponent<UnitylityCamera>();
		}

#if UNITY_EDITOR
		void Start() { } // Display enabled checkbox
#endif

		/// <summary>
		/// Sometimes the output value of scroll is a Vector2, then we use the y value. It just happens to be y for normal scroll.
		/// </summary>
		public void Zoom(Vector2 amount) {
			Zoom(amount.y);
		}

		public virtual void Zoom(float amount) {
			if (enabled && amount != 0) {
				var delta = amount;
				if (delta < 0) {
					if (mucam.distance <= range.y) {
						var v = (normalize ? Mathf.Clamp(delta, -1, 1) : delta) * Mathf.Max(mucam.distance * multiplier - mucam.distance, minStep);
						mucam.distance -= v;
						mucam.distance = Mathf.Min(mucam.distance, range.y);
					}
				} else {
					if (mucam.distance >= range.x) {
						float v = (normalize ? Mathf.Clamp(delta, -1, 1) : delta) * Mathf.Max(mucam.distance - mucam.distance / multiplier, minStep);
						mucam.distance -= v;
						mucam.distance = Mathf.Max(mucam.distance, range.x);
					}
				}
			}
		}

	}

}