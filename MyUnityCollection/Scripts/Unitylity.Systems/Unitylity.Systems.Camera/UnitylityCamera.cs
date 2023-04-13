
namespace Unitylity.Systems.Camera {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Unitylity.Extensions;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Camera) + "/" + nameof(UnitylityCamera))]
#endif
	[DefaultExecutionOrder(-1000)]
	[RequireComponent(typeof(Camera))]
	public class UnitylityCamera : MonoBehaviour {

		[HideInInspector] public Camera cam;
		public GameObject target;
		public Vector3 center;
		public float distance = 1;
		public float horRot;
		public float verRot;
		public Vector3 displacement;

		[Range(0, 0.9999f)] public float moveSmooth = 0.85f;
		[Range(0, 0.9999f)] public float zoomSmooth = 0.85f;
		[Range(0, 0.9999f)] public float rotSmooth = 0.85f;
		[Range(0, 0.9999f)] public float displacementSmooth = 0.85f;

		GameObject prevTarget;
		Vector3 _displacement;
		Vector3 _center;
		float _distance;
		float _HorRot;
		float _VerRot;

		void OnValidate() {
			ResetValues();
			LateUpdate();
		}

		void Awake() {
			cam = GetComponent<Camera>();
		}

		void Start() {
			ResetValues();
		}

		void LateUpdate() {
			// Reset addition on target change
			if (target != prevTarget) {
				displacement = Vector3.zero;
				_displacement = displacement;
			}
			prevTarget = target;

			var pivot = Vector3.zero;
			var point = Vector3.zero;

			// Follow target
			if (target) {
				center = target.transform.position;
				_center = Vector3.MoveTowards(center, _center, Vector3.Distance(center, _center) * moveSmooth);
				pivot += _center;
			}

			// Evaluate zoom
			_distance += (distance - _distance) * (1f - zoomSmooth);

			// Evaluate rotations
			_HorRot += (horRot - _HorRot) * (1f - rotSmooth);
			_VerRot += (verRot - _VerRot) * (1f - rotSmooth);

			// Apply distance
			point = point.SetY(_distance);
			// Apply rotation around x axis
			point = Quaternion.AngleAxis(_VerRot - 90, Vector3.right) * point;
			// Apply rotation around y axis
			point = Quaternion.AngleAxis(_HorRot, Vector3.up) * point;

			// Smooth addition
			_displacement = Vector3.MoveTowards(displacement, _displacement, Vector3.Distance(displacement, _displacement) * displacementSmooth);
			point += _displacement;

			transform.position = pivot + point;
			transform.rotation = Quaternion.Euler(_VerRot, _HorRot, 0);
		}

		void ResetValues() {
			prevTarget = target;
			_displacement = displacement;
			_distance = distance;
			_HorRot = horRot;
			_VerRot = verRot;
		}

	}

}