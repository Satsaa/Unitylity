
namespace Unitylity.Systems.Camera {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

#if (UNITYLITY_HIDE_COMPONENTS || UNITYLITY_HIDE_SYSTEM_COMPONENTS || UNITYLITY_HIDE_SYSTEM_CAMERA)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Camera) + "/" + nameof(UnitylityCameraDrag))]
#endif
	[RequireComponent(typeof(UnitylityCamera))]
	public class UnitylityCameraDrag : MonoBehaviour {

		[SerializeField, HideInInspector] UnitylityCamera Unitylityam;
		public LayerMask mask;

		public bool raycastPlaneNormal;
		public Vector3 planeNormal = Vector3.up;
		public bool raycastPlanePoint;
		public Vector3 planePoint;

		protected bool dragging;
		Vector3 mousePosition;
		Vector3 rayOrigin;
		Vector3 prev;
		Plane plane => new(planeNormal, planePoint);

		void Awake() {
			Unitylityam = gameObject.GetComponent<UnitylityCamera>();
		}

#if UNITY_EDITOR
		void Start() { } // Display enabled checkbox
#endif

		/// <summary> Sets the current drag position and moves the camera. </summary>
		public virtual void Drag(Vector2 position) {
			mousePosition = position;
			if (dragging) {
				if (!RefreshDragPoint(plane, out var current)) return;
				var dif = prev - current;
				Unitylityam.displacement += dif;
				prev = current;
			}
		}

		/// <summary> Starts or ends dragging. </summary>
		public void SetDragging(bool dragging) {
			if (dragging) {
				StartDrag();
			} else {
				EndDrag();
			}
		}

		protected virtual void StartDrag() {
			if (dragging != (dragging = true)) {
				rayOrigin = transform.position;

				if (raycastPlaneNormal || raycastPlanePoint) {

					var ray = Unitylityam.cam.ScreenPointToRay(mousePosition);
					ray.origin = rayOrigin;

					if (Physics.Raycast(ray, out var hit, mask)) {
						if (raycastPlaneNormal) planeNormal = hit.normal;
						if (raycastPlanePoint) planePoint = hit.point;
					}
				}

				RefreshDragPoint(plane, out prev);
			}
		}

		protected virtual void EndDrag() {
			dragging = false;
		}

		private bool RefreshDragPoint(Plane plane, out Vector3 point) {
			var ray = Unitylityam.cam.ScreenPointToRay(mousePosition);
			ray.origin = rayOrigin;

			var res = (plane.Raycast(ray, out float enter));
			if (res) point = ray.origin + ray.direction * enter;
			else point = Vector3.zero;
			return res;
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Systems.Camera.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(UnitylityCameraDrag), true)]
	public class UnitylityCameraDragEditor : Editor {

		UnitylityCameraDrag t => (UnitylityCameraDrag)target;

		SerializedProperty raycastPlaneNormal;
		SerializedProperty raycastPlanePoint;
		SerializedProperty planeNormal;
		SerializedProperty planePoint;
		SerializedProperty mask;

		void OnEnable() {
			raycastPlaneNormal = serializedObject.FindProperty(nameof(UnitylityCameraDrag.raycastPlaneNormal));
			raycastPlanePoint = serializedObject.FindProperty(nameof(UnitylityCameraDrag.raycastPlanePoint));
			planeNormal = serializedObject.FindProperty(nameof(UnitylityCameraDrag.planeNormal));
			planePoint = serializedObject.FindProperty(nameof(UnitylityCameraDrag.planePoint));
			mask = serializedObject.FindProperty(nameof(UnitylityCameraDrag.mask));
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			ScriptField(serializedObject);

			// Normals
			EditorGUILayout.PropertyField(raycastPlaneNormal);
			if (!raycastPlaneNormal.boolValue) EditorGUILayout.PropertyField(planeNormal);

			// Points
			EditorGUILayout.PropertyField(raycastPlanePoint);
			if (!raycastPlanePoint.boolValue) EditorGUILayout.PropertyField(planePoint);

			// Shared
			if (raycastPlaneNormal.boolValue || raycastPlanePoint.boolValue) {
				EditorGUILayout.PropertyField(mask);
			}

			DrawPropertiesExcluding(serializedObject,
				script,
				raycastPlaneNormal.name,
				raycastPlanePoint.name,
				planeNormal.name,
				planePoint.name,
				mask.name
			);

			serializedObject.ApplyModifiedProperties();
		}

	}

}
#endif