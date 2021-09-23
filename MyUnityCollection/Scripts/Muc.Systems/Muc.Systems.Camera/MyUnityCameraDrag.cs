

namespace Muc.Systems.Camera {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.Camera) + "/" + nameof(MyUnityCameraDrag))]
#endif
	[RequireComponent(typeof(MyUnityCamera))]
	public class MyUnityCameraDrag : MonoBehaviour {

		[SerializeField, HideInInspector] MyUnityCamera mucam;
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
			mucam = gameObject.GetComponent<MyUnityCamera>();
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
				mucam.displacement += dif;
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

					var ray = mucam.cam.ScreenPointToRay(mousePosition);
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
			var ray = mucam.cam.ScreenPointToRay(mousePosition);
			ray.origin = rayOrigin;

			var res = (plane.Raycast(ray, out float enter));
			if (res) point = ray.origin + ray.direction * enter;
			else point = Vector3.zero;
			return res;
		}
	}

}


#if UNITY_EDITOR
namespace Muc.Systems.Camera.Editor {

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using Object = UnityEngine.Object;
	using static Muc.Editor.PropertyUtil;
	using static Muc.Editor.EditorUtil;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(MyUnityCameraDrag), true)]
	public class MyUnityCameraDragEditor : Editor {

		MyUnityCameraDrag t => (MyUnityCameraDrag)target;

		SerializedProperty raycastPlaneNormal;
		SerializedProperty raycastPlanePoint;
		SerializedProperty planeNormal;
		SerializedProperty planePoint;
		SerializedProperty mask;

		void OnEnable() {
			raycastPlaneNormal = serializedObject.FindProperty(nameof(MyUnityCameraDrag.raycastPlaneNormal));
			raycastPlanePoint = serializedObject.FindProperty(nameof(MyUnityCameraDrag.raycastPlanePoint));
			planeNormal = serializedObject.FindProperty(nameof(MyUnityCameraDrag.planeNormal));
			planePoint = serializedObject.FindProperty(nameof(MyUnityCameraDrag.planePoint));
			mask = serializedObject.FindProperty(nameof(MyUnityCameraDrag.mask));
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