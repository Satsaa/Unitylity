
namespace Unitylity.Systems.Camera {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Security.Cryptography;
	using UnityEngine;
	using Unitylity.Editor;

#if UNITYLITY_SYSTEMS_CAMERA_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Camera) + "/" + nameof(UnitylityCameraDrag))]
#endif
	[RequireComponent(typeof(UnitylityCamera))]
	public class UnitylityCameraDrag : MonoBehaviour {

		public class DragData {
			public bool dragging;

			public Vector3 position;
			public Vector2 pointer;
		}

		public class MultiDragData {
			public float startAngle;
			public float angle;
			public float startDistance;
			public float distance;
		}

		[SerializeField, HideInInspector] UnitylityCamera ucam;
		public LayerMask mask;

		public DragData primaryData = new();
		public DragData secondaryData = new();

		public MultiDragData multiDragData = new();

		public int dragCount => (primaryData.dragging ? 1 : 0) + (secondaryData.dragging ? 1 : 0);

		public bool raycastPlaneNormal;
		public Vector3 planeNormal = Vector3.up;
		public bool raycastPlanePoint;
		public Vector3 planePoint;

		protected Plane plane;

		public bool showDebugGizmos;

		private Vector3 originalPosition;
		private Quaternion originalRotation;

		void Awake() {
			ucam = gameObject.GetComponent<UnitylityCamera>();
		}

#if UNITY_EDITOR
		void Start() { } // Display enabled checkbox
#endif

		/// <summary> Sets the current drag position and moves the camera. </summary>
		public virtual void SetPointer(Vector2 position) {
			primaryData.pointer = position;
			switch (dragCount) {
				case 0:
					if (GetHitPoint(primaryData, out var point))
						primaryData.position = point;
					break;
				case 1: DoSingleDrag(primaryData); break;
				case 2: DoDualDrag(); break;
			}
		}

		/// <summary> Sets the current drag position and moves the camera. </summary>
		public virtual void SetPointerSecondary(Vector2 position) {
			secondaryData.pointer = position;
			switch (dragCount) {
				case 0:
					if (GetHitPoint(secondaryData, out var point))
						secondaryData.position = point;
					break;
				case 1: DoSingleDrag(secondaryData); break;
				case 2: DoDualDrag(); break;
			}
		}

		protected void DoSingleDrag(DragData data) {
			if (data.dragging) {
				if (!GetHitPoint(data, out var point)) return;
				var diff = data.position - point;
				ucam.displacement += diff;
				data.position = point;
			}
		}

		protected void DoDualDrag() {
			if (!GetHitPoint(primaryData, out var point1)) return;
			if (!GetHitPoint(secondaryData, out var point2)) return;

			var angle = GetAngle();
			var angleDiff = multiDragData.angle - angle;
			if (angleDiff > 180) {
				angleDiff -= 360;
			} else if (angleDiff < -180) {
				angleDiff += 360;
			}
			ucam.horRot += angleDiff;
			multiDragData.angle = angle;

			var distance = GetDistance();
			var distanceRatio = multiDragData.distance / distance;
			ucam.distance *= distanceRatio;
			multiDragData.distance = distance;

			var oldAvgPosition = (primaryData.position + secondaryData.position) / 2;
			var newAvgPosition = (point1 + point2) / 2;
			var diff = oldAvgPosition - newAvgPosition;
			var angleRotation = Quaternion.AngleAxis(multiDragData.startAngle - angle, plane.normal);
			var distanceScaling = multiDragData.startDistance / distance;
			ucam.displacement += angleRotation * diff * distanceScaling;
			primaryData.position = point1;
			secondaryData.position = point2;

		}

		/// <summary> Starts or ends dragging. </summary>
		public void SetDragging(bool dragging) {
			if (dragging) StartDrag(primaryData);
			else EndDrag(primaryData);
		}

		/// <summary> Starts or ends dragging. </summary>
		public void SetDraggingSecondary(bool dragging) {
			if (dragging) StartDrag(secondaryData);
			else EndDrag(secondaryData);
		}

		protected virtual void StartDrag(DragData data) {
			StartCoroutine(StartDragNextFrame(data));
		}

		protected virtual IEnumerator StartDragNextFrame(DragData data) {
			yield return new WaitForEndOfFrame();
			if (data.dragging != (data.dragging = true)) {

				if (dragCount == 1) {
					if (raycastPlaneNormal || raycastPlanePoint) {

						var ray = ucam.cam.ScreenPointToRay(data.pointer);
						ray.origin = transform.position;

						if (Physics.Raycast(ray, out var hit, mask)) {
							if (raycastPlaneNormal) planeNormal = hit.normal;
							if (raycastPlanePoint) planePoint = hit.point;
						}
					}
					plane = new(planeNormal, planePoint - ucam.transform.position);
					ucam.cam.transform.GetPositionAndRotation(out originalPosition, out originalRotation);
				}

				if (GetHitPoint(data, out var point)) {
					data.position = data.position = point;
					if (dragCount == 2) {
						multiDragData.startAngle = multiDragData.angle = GetAngle();
						multiDragData.startDistance = multiDragData.distance = GetDistance();
					}
				} else {
					data.dragging = false;
				}
			}
		}

		protected virtual void EndDrag(DragData data) {
			if (dragCount == 2) {
				var otherData = data == primaryData ? secondaryData : primaryData;
				ucam.cam.transform.GetPositionAndRotation(out originalPosition, out originalRotation);
				if (GetHitPoint(otherData, out var point)) {
					otherData.position = otherData.position = point;
				} else {
					otherData.dragging = false;
				}
			}
			data.dragging = false;
		}

		private bool GetHitPoint(DragData data, out Vector3 point) {
			var ray = CreateRayFromOriginalTransform(data.pointer);
			ray.origin = Vector3.zero;
			if (plane.Raycast(ray, out float enter)) {
				point = ray.origin + ray.direction * enter;
				return true;
			}
			point = default;
			return false;
		}

		private float GetAngle() {
			var baselineDirection = Vector3.ProjectOnPlane(Vector3.right, plane.normal).normalized;
			var directionToSecondary = (secondaryData.position - primaryData.position).normalized;
			return Vector3.SignedAngle(baselineDirection, directionToSecondary, plane.normal);
		}

		private float GetDistance() {
			// You might note that the distance is scaled based on screen distance, instead of world distance, and that is for now the way.
			return Vector2.Distance(primaryData.pointer, secondaryData.pointer);
		}

		// Method to create a ray from a screen point using the camera's original transform
		public Ray CreateRayFromOriginalTransform(Vector3 screenPoint) {
			// Store current transform
			ucam.cam.transform.GetPositionAndRotation(out var currentPosition, out var currentRotation);

			// Set camera to original transform
			ucam.cam.transform.SetPositionAndRotation(originalPosition, originalRotation);

			// Create the ray
			Ray ray = ucam.cam.ScreenPointToRay(screenPoint);

			// Reset camera to current transform
			ucam.cam.transform.SetPositionAndRotation(currentPosition, currentRotation);
			return ray;
		}

		void OnDrawGizmos() {
			if (showDebugGizmos) {
				ShowGizmo(primaryData);
				ShowGizmo(secondaryData);
				void ShowGizmo(DragData data) {
					if (data.dragging) {
						var ray = ucam.cam.ScreenPointToRay(data.pointer);
						ray.origin = default;
						var res = plane.Raycast(ray, out float enter);
						var wsHitPoint = ray.origin + ray.direction * enter + transform.position;
						if (res) {
							Gizmos.DrawSphere(wsHitPoint, 0.25f);
							Gizmos.DrawRay(wsHitPoint, plane.normal * 1);
						}
						ray.origin = transform.position;
						Gizmos.DrawRay(ray);
						Gizmos.DrawRay(new(wsHitPoint, Quaternion.Euler(90f, multiDragData.angle - 90f + ucam.cam.transform.rotation.eulerAngles.y, 0f) * plane.normal));
					}
				}
			}
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