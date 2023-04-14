
namespace Unitylity.Systems.RenderImages {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using Unitylity.Extensions;

#if UNITYLITY_SYSTEMS_RENDERIMAGES_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.RenderImages) + "/" + nameof(RenderImage))]
#endif
	public class RenderImage : RawImage {

		[SerializeField] protected internal RenderObject renderPrefab;

		[SerializeField, Range(0, 1)] protected internal float resolutionScale = 1;

		[SerializeField] protected internal Vector2 maxResolution = new(float.PositiveInfinity, float.PositiveInfinity);

		[SerializeField, HideInInspector] protected internal RenderObject renderObject;

		public Vector2 rectSize => rectTransform.rect.size * scale;
		public float ratio { get { var rect = rectTransform.rect.size; return rect.x / rect.y; } }
		public Vector2 preferredResolution => (rectTransform.rect.size * scale * resolutionScale).Max(1).Min(maxResolution);
		[SerializeField, HideInInspector] private Vector3 scale;

		protected override void Awake() {
			base.Awake();
			if (Application.isPlaying) {
				renderObject = null;
			}
		}

		protected override void OnEnable() {
			base.OnEnable();
			if (Application.isPlaying && renderPrefab) {
				if (!renderObject) {
					renderObject = RenderObjects.instance.GetObject(renderPrefab);
					renderObject.AddDependent(this);
					var canvas = GetComponentInParent<Canvas>();
					if (canvas && canvas.rootCanvas) scale = canvas.rootCanvas.transform.localScale;
				}
				renderObject.gameObject.SetActive(true);
				renderObject.enabled = true;
				renderObject.doEnableCheck = true;
				renderObject.doValueCheck = true;
				renderObject.doSetImageValues = true;
			}
		}

		protected override void OnDisable() {
			base.OnDisable();
			if (Application.isPlaying) {
				if (renderObject) {
					renderObject.doEnableCheck = true;
					renderObject.doValueCheck = true;
				}
			}
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			if (Application.isPlaying) {
				if (renderObject != null) {
					renderObject.RemoveDependent(this);
				}
			}
		}

		protected override void OnRectTransformDimensionsChange() {
			base.OnRectTransformDimensionsChange();
			if (Application.isPlaying) {
				if (renderObject) {
					renderObject.doValueCheck = true;
					renderObject.doSetImageValues = true;
					var canvas = GetComponentInParent<Canvas>();
					if (canvas && canvas.rootCanvas) scale = canvas.rootCanvas.transform.localScale;
				}
			}
		}

		public virtual void SetRenderObject(RenderObject renderPrefab) {
			if (this.renderPrefab == renderPrefab) return;
			this.renderPrefab = renderPrefab;
			if (renderObject) {
				renderObject.RemoveDependent(this);
				renderObject.doEnableCheck = true;
				renderObject.doValueCheck = true;
				renderObject.doSetImageValues = true;
			}
			if (isActiveAndEnabled) OnEnable();
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Systems.RenderImages.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEditor.UI;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(RenderImage), true)]
	public class RenderImageEditor : RawImageEditor {

		new RenderImage target => (RenderImage)base.target;
		new IEnumerable<RenderImage> targets => base.targets.Cast<RenderImage>();

		SerializedProperty renderPrefab;
		SerializedProperty resolutionScale;
		SerializedProperty texture;

		protected override void OnEnable() {
			base.OnEnable();
			renderPrefab = serializedObject.FindProperty(nameof(RenderImage.renderPrefab));
			resolutionScale = serializedObject.FindProperty(nameof(RenderImage.resolutionScale));
			texture = serializedObject.FindProperty("m_Texture");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			base.serializedObject.Update();
			EditorGUILayout.PropertyField(texture);
			AppearanceControlsGUI();
			RaycastControlsGUI();
			MaskableControlsGUI();

			Space();

			using (DisabledScope(Application.isPlaying)) {
				PropertyField(renderPrefab);
			}

			EditorGUI.BeginChangeCheck();

			DrawPropertiesExcluding(serializedObject,
				script,
				renderPrefab.name,
				"m_Texture",
				"m_Texture",
				"m_UVRect",
				"m_OnCullStateChanged",
				m_Color.name,
				m_Material.name,
				m_RaycastTarget.name,
				m_RaycastPadding.name,
				m_Maskable.name
			);

			serializedObject.ApplyModifiedProperties();
			if (EditorGUI.EndChangeCheck()) {
				foreach (var target in targets) {
					if (target.renderObject) target.renderObject.doValueCheck = true;
				}
			}

			// Field($"rectSize = {target.rectSize}");
			// Field($"ratio = {target.ratio}");
			// Field($"preferredResolution = {target.preferredResolution}");
		}

	}

}
#endif