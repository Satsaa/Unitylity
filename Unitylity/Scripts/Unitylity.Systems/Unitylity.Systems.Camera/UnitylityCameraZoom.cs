
namespace Unitylity.Systems.Camera {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

#if UNITYLITY_SYSTEMS_CAMERA_HIDDEN
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
		private UnitylityCamera ucam;


		protected void Awake() {
			ucam = gameObject.GetComponent<UnitylityCamera>();
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
					if (ucam.distance <= range.y) {
						var v = (normalize ? Mathf.Clamp(delta, -1, 1) : delta) * Mathf.Max(ucam.distance * multiplier - ucam.distance, minStep);
						ucam.distance -= v;
						ucam.distance = Mathf.Min(ucam.distance, range.y);
					}
				} else {
					if (ucam.distance >= range.x) {
						float v = (normalize ? Mathf.Clamp(delta, -1, 1) : delta) * Mathf.Max(ucam.distance - ucam.distance / multiplier, minStep);
						ucam.distance -= v;
						ucam.distance = Mathf.Max(ucam.distance, range.x);
					}
				}
			}
		}

	}

}