

namespace Muc.Systems.RenderImages {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Muc.Extensions;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.RenderImages) + "/" + nameof(RenderImage))]
#endif
	public class RenderImage : RawImage {

		[SerializeField] protected internal RenderObject renderPrefab;
		[SerializeField] protected internal bool shareRenderPrefab;

		[SerializeField, Range(0, 1)] protected internal float renderScale = 1;

		[SerializeField] protected internal Antialiasing antialiasing = Antialiasing.None;
		[SerializeField] protected internal RenderTextureFormat format = RenderTextureFormat.ARGB32;
		[SerializeField] protected internal DebthBits debthBits = DebthBits.Bits24Stencil;
		[SerializeField] protected internal bool enableMipMaps = false;
		[SerializeField] protected internal bool autoGenerateMips = false;
		[SerializeField] protected internal bool dynamicScaling = false;
		[SerializeField] protected internal FilterMode filterMode = FilterMode.Point;
		[SerializeField, Range(0, 16)] protected internal int anisoLevel = 0;

		[SerializeField, HideInInspector] protected internal RenderObject renderObject;

		public Vector2 rawResolution => (rectTransform.rect.size * scale);
		public Vector2Int resolution => (rectTransform.rect.size * scale).Mul(renderScale).RoundInt().Max(1);
		private Vector3 scale;

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
					renderObject = RenderObjects.instance.GetObject(renderPrefab, shareRenderPrefab);
					renderObject.AddDependent(this);
					var canvas = GetComponentInParent<Canvas>();
					if (canvas && canvas.rootCanvas) scale = canvas.rootCanvas.transform.localScale;
				}
				renderObject.gameObject.SetActive(true);
				renderObject.doValueCheck = true;
			}
		}

		protected override void OnDisable() {
			base.OnDisable();
			if (Application.isPlaying) {
				if (renderObject) {
					renderObject.doCheckEnable = true;
					renderObject.doValueCheck = true;
				}
			}
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			if (Application.isPlaying) {
				if (renderObject) {
					renderObject.RemoveDependent(this);
				}
			}
		}

		protected override void OnRectTransformDimensionsChange() {
			base.OnRectTransformDimensionsChange();
			if (Application.isPlaying) {
				if (renderObject) {
					renderObject.doValueCheck = true;
					var canvas = GetComponentInParent<Canvas>();
					if (canvas && canvas.rootCanvas) scale = canvas.rootCanvas.transform.localScale;
				}
			}
		}

		public virtual void SetRenderObject(RenderObject renderPrefab) {
			if (this.renderPrefab == renderPrefab) return;
			if (renderObject) {
				renderObject.RemoveDependent(this);
				renderObject.doCheckEnable = true;
				renderObject.doValueCheck = true;
			}
			this.renderPrefab = renderPrefab;
			if (isActiveAndEnabled) OnEnable();
		}

		protected internal virtual RenderTexture CreateTexture(Vector2Int resolution) {
			if (!Application.isPlaying) return null;

			var descriptor = new RenderTextureDescriptor(resolution.x, resolution.y, format, (int)debthBits) {
				useMipMap = enableMipMaps,
				autoGenerateMips = autoGenerateMips && enableMipMaps,
				msaaSamples = (int)antialiasing,
				useDynamicScale = dynamicScaling
			};

			var res = new RenderTexture(descriptor) {
				name = $"{nameof(RenderImage)} ({resolution.x}x{resolution.y})",
				filterMode = filterMode,
				anisoLevel = debthBits == DebthBits.None ? anisoLevel : 0
			};

			return res;
		}


		protected internal enum DebthBits {
			None = 0,
			Bits16 = 16,
			Bits24Stencil = 24,
			Bits32Stencil = 32
		}

		protected internal enum Antialiasing {
			None = 1,
			AA2 = 2,
			AA4 = 4,
			AA8 = 8
		}

	}

}

#if UNITY_EDITOR
namespace Muc.Systems.RenderImages.Editor {

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using UnityEditor.UI;
	using Object = UnityEngine.Object;
	using static Muc.Editor.PropertyUtil;
	using static Muc.Editor.EditorUtil;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(RenderImage), true)]
	public class RenderImageEditor : RawImageEditor {

		RenderImage t => (RenderImage)target;

		SerializedProperty renderPrefab;
		SerializedProperty shareRenderPrefab;
		SerializedProperty renderScale;
		SerializedProperty antialiasing;
		SerializedProperty format;
		SerializedProperty debthBits;
		SerializedProperty enableMipMaps;
		SerializedProperty autoGenerateMips;
		SerializedProperty dynamicScaling;
		SerializedProperty filterMode;
		SerializedProperty anisoLevel;

		protected override void OnEnable() {
			base.OnEnable();
			renderPrefab = serializedObject.FindProperty(nameof(RenderImage.renderPrefab));
			shareRenderPrefab = serializedObject.FindProperty(nameof(RenderImage.shareRenderPrefab));
			renderScale = serializedObject.FindProperty(nameof(RenderImage.renderScale));
			antialiasing = serializedObject.FindProperty(nameof(RenderImage.antialiasing));
			format = serializedObject.FindProperty(nameof(RenderImage.format));
			debthBits = serializedObject.FindProperty(nameof(RenderImage.debthBits));
			enableMipMaps = serializedObject.FindProperty(nameof(RenderImage.enableMipMaps));
			autoGenerateMips = serializedObject.FindProperty(nameof(RenderImage.autoGenerateMips));
			dynamicScaling = serializedObject.FindProperty(nameof(RenderImage.dynamicScaling));
			filterMode = serializedObject.FindProperty(nameof(RenderImage.filterMode));
			anisoLevel = serializedObject.FindProperty(nameof(RenderImage.anisoLevel));
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			serializedObject.Update();

			EditorGUILayout.Space();

			using (DisabledScope(Application.isPlaying)) {
				EditorGUILayout.PropertyField(renderPrefab);
				EditorGUILayout.PropertyField(shareRenderPrefab);
			}

			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(renderScale);
			EditorGUILayout.PropertyField(antialiasing);
			EditorGUILayout.PropertyField(format);
			EditorGUILayout.PropertyField(debthBits);
			EditorGUILayout.PropertyField(enableMipMaps);
			using (DisabledScope(!enableMipMaps.boolValue)) EditorGUILayout.PropertyField(autoGenerateMips);
			EditorGUILayout.PropertyField(dynamicScaling);
			EditorGUILayout.PropertyField(filterMode);
			using (DisabledScope(debthBits.intValue != 0)) EditorGUILayout.PropertyField(anisoLevel);

			DrawPropertiesExcluding(serializedObject,
				script,
				renderPrefab.name,
				shareRenderPrefab.name,
				renderScale.name,
				antialiasing.name,
				format.name,
				debthBits.name,
				enableMipMaps.name,
				autoGenerateMips.name,
				dynamicScaling.name,
				filterMode.name,
				anisoLevel.name,
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
				foreach (RenderImage target in targets) {
					if (target.renderObject) target.renderObject.doValueCheck = true;
				}
			}
		}
	}
}
#endif