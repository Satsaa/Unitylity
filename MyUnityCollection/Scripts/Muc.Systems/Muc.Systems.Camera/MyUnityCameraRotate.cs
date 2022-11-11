
namespace Muc.Systems.Camera {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.Camera) + "/" + nameof(MyUnityCameraRotate))]
#endif
	[RequireComponent(typeof(MyUnityCamera))]
	public class MyUnityCameraRotate : MonoBehaviour {

		public float multiplier = 0.1f;
		public bool limitVerticalRotation = true;
		public float rotationLimit = 89.9f;

		[SerializeField, HideInInspector]
		private MyUnityCamera mucam;

		protected void Awake() {
			mucam = gameObject.GetComponent<MyUnityCamera>();
		}

#if UNITY_EDITOR
		void Start() { } // Display enabled checkbox
#endif

		public void Rotate(Vector2 delta) {
			if (enabled) {
				mucam.horRot -= (delta.x) * multiplier;
				mucam.verRot += (delta.y) * multiplier;
				if (limitVerticalRotation) {
					mucam.verRot = Mathf.Clamp(mucam.verRot, -rotationLimit, rotationLimit);
				}
			}
		}

	}

#if UNITY_EDITOR
	namespace Editor {

		using System;
		using System.Collections.Generic;
		using System.Linq;
		using UnityEditor;
		using UnityEngine;
		using static Muc.Editor.EditorUtil;
		using static Muc.Editor.PropertyUtil;
		using Object = UnityEngine.Object;

		[CanEditMultipleObjects]
		[CustomEditor(typeof(MyUnityCameraRotate), true)]
		public class MyUnityCameraRotateEditor : Editor {

			MyUnityCameraRotate t => (MyUnityCameraRotate)target;

			SerializedProperty multiplier;
			SerializedProperty limitVerticalRotation;
			SerializedProperty rotationLimit;

			void OnEnable() {
				multiplier = serializedObject.FindProperty(nameof(MyUnityCameraRotate.multiplier));
				limitVerticalRotation = serializedObject.FindProperty(nameof(MyUnityCameraRotate.limitVerticalRotation));
				rotationLimit = serializedObject.FindProperty(nameof(MyUnityCameraRotate.rotationLimit));
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
}