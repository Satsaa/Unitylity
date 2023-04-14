
namespace Unitylity.Systems.Camera {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

#if UNITYLITY_SYSTEMS_CAMERA_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Camera) + "/" + nameof(UnitylityCameraRotate))]
#endif
	[RequireComponent(typeof(UnitylityCamera))]
	public class UnitylityCameraRotate : MonoBehaviour {

		public float multiplier = 0.1f;
		public bool limitVerticalRotation = true;
		public float rotationLimit = 89.9f;

		[SerializeField, HideInInspector]
		private UnitylityCamera Unitylityam;

		protected void Awake() {
			Unitylityam = gameObject.GetComponent<UnitylityCamera>();
		}

#if UNITY_EDITOR
		void Start() { } // Display enabled checkbox
#endif

		public void Rotate(Vector2 delta) {
			if (enabled) {
				Unitylityam.horRot -= (delta.x) * multiplier;
				Unitylityam.verRot += (delta.y) * multiplier;
				if (limitVerticalRotation) {
					Unitylityam.verRot = Mathf.Clamp(Unitylityam.verRot, -rotationLimit, rotationLimit);
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
	[CustomEditor(typeof(UnitylityCameraRotate), true)]
	public class UnitylityCameraRotateEditor : Editor {

		UnitylityCameraRotate t => (UnitylityCameraRotate)target;

		SerializedProperty multiplier;
		SerializedProperty limitVerticalRotation;
		SerializedProperty rotationLimit;

		void OnEnable() {
			multiplier = serializedObject.FindProperty(nameof(UnitylityCameraRotate.multiplier));
			limitVerticalRotation = serializedObject.FindProperty(nameof(UnitylityCameraRotate.limitVerticalRotation));
			rotationLimit = serializedObject.FindProperty(nameof(UnitylityCameraRotate.rotationLimit));
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			ScriptField(serializedObject);

			// Multiplier
			EditorGUILayout.PropertyField(multiplier);

			// Vertical rotation limiting
			EditorGUILayout.PropertyField(limitVerticalRotation);
			if (limitVerticalRotation.boolValue) EditorGUILayout.PropertyField(rotationLimit);

			DrawPropertiesExcluding(serializedObject,
				script,
				multiplier.name,
				limitVerticalRotation.name,
				rotationLimit.name
			);

			serializedObject.ApplyModifiedProperties();
		}

	}

}
#endif