
namespace Unitylity.Systems.RenderImages {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.Rendering.Universal;
	using Unitylity.Extensions;

#if UNITYLITY_SYSTEMS_RENDERIMAGES_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.RenderImages) + "/" + nameof(RenderObject))]
#endif
	public class RenderObject : MonoBehaviour {

		#region User Properties

		[SerializeField] private UpdateMode _updateMode = UpdateMode.Initialize;
		public UpdateMode updateMode {
			get => _updateMode;
			set {
				if (_updateMode == value) return;
				_updateMode = value;
				if (camera) camera.enabled = _updateMode == UpdateMode.Always;
			}
		}

		[SerializeField] private Camera _camera;

#if UNITY_EDITOR
		new public Camera camera {
#else
		public Camera camera {
#endif
			get => _camera;
			set {
				if (_camera == value) return;
				_camera = value;
				doTextureReset = value;
				if (value) {
					_camera.targetTexture = rt;
					_camera.enabled = _updateMode == UpdateMode.Always;
				}
			}
		}


		[field: SerializeField, Tooltip("Disable or enable the " + nameof(RenderObject) + " depending on the state of the " + nameof(RenderImage) + "s. The Camera and this Component will always be disabled.")]
		public bool autoDisable { get; set; } = true;

		[field: SerializeField, Tooltip("When the RenderImages are removed, instead of destroying, put the RenderObject in a pool for reuse.\n\n <0  = infinite")]
		public int poolSize { get; private set; }

		[field: SerializeField, Tooltip("Multiple " + nameof(RenderImage) + "s will use the same RenderTexture.")]
		public bool shared { get; private set; }


		[SerializeField, Range(0, 1)] float _renderScale = 1;
		public float renderScale {
			get => _renderScale;
			set {
				if (_renderScale == value) return;
				_renderScale = value;
				doTextureReset = true;
			}
		}

		[SerializeField] Antialiasing _antialiasing = Antialiasing.None;
		public Antialiasing antialiasing {
			get => _antialiasing;
			set {
				if (_antialiasing == value) return;
				_antialiasing = value;
				doTextureReset = true;
			}
		}

		[SerializeField] RenderTextureFormat _format = RenderTextureFormat.ARGB32;
		public RenderTextureFormat format {
			get => _format;
			set {
				if (_format == value) return;
				_format = value;
				doTextureReset = true;
			}
		}

		[SerializeField] DebthBits _debthBits = DebthBits.Bits24Stencil;
		public DebthBits debthBits {
			get => _debthBits;
			set {
				if (_debthBits == value) return;
				_debthBits = value;
				doTextureReset = true;
			}
		}

		[SerializeField] bool _enableMipMaps = false;
		public bool enableMipMaps {
			get => _enableMipMaps;
			set {
				if (_enableMipMaps == value) return;
				_enableMipMaps = value;
				doTextureReset = true;
			}
		}

		[SerializeField] bool _autoGenerateMips = false;
		public bool autoGenerateMips {
			get => _autoGenerateMips;
			set {
				if (_autoGenerateMips == value) return;
				_autoGenerateMips = value;
				doTextureReset = true;
			}
		}

		[SerializeField] bool _dynamicScaling = false;
		public bool dynamicScaling {
			get => _dynamicScaling;
			set {
				if (_dynamicScaling == value) return;
				_dynamicScaling = value;
				doTextureReset = true;
			}
		}

		[SerializeField] FilterMode _filterMode = FilterMode.Point;
		public FilterMode filterMode {
			get => _filterMode;
			set {
				if (_filterMode == value) return;
				_filterMode = value;
				doTextureReset = true;
			}
		}

		[SerializeField, Range(0, 16)] int _anisoLevel = 0;
		public int anisoLevel {
			get => _anisoLevel;
			set {
				if (_anisoLevel == value) return;
				_anisoLevel = value;
				doTextureReset = true;
			}
		}

		#endregion


		[SerializeField, HideInInspector] internal RenderObject prefab;
		[SerializeField, HideInInspector] protected RenderTexture rt;

		[field: SerializeField, HideInInspector]
		public List<RenderImage> renderImages { get; private set; } = new List<RenderImage>();


		public float maxRatio => renderImages.Aggregate(0f, (acc, v) => !v.enabled ? acc : Mathf.Max(acc, v.ratio));
		public float maxHeight => Mathf.Max(1f, renderImages.Aggregate(0f, (acc, v) => !v.enabled ? acc : Mathf.Max(acc, v.preferredResolution.y)) * renderScale);

		public Vector2Int targetRes {
			get {
				var maxRatio = this.maxRatio;
				var maxHeight = this.maxHeight;
				return new Vector2Int(
					Mathf.CeilToInt(maxHeight * maxRatio),
					Mathf.CeilToInt(maxHeight)
				).Max(1);
			}
		}

		public bool doEnableCheck { get; set; }
		public bool doValueCheck { get; set; }
		public bool doTextureReset { get; set; }
		public bool doSetImageValues { get; set; }


		protected void Reset() {
			if (camera) camera = GetComponentInChildren<Camera>();
		}

		protected void Awake() {
			if (camera) camera.enabled = false;
		}

		protected void Start() {
			if (camera) camera.enabled = updateMode == UpdateMode.Always;
		}

		protected void LateUpdate() {
			if (doEnableCheck) {
				doEnableCheck = false;
				var active = renderImages.Any(v => v.isActiveAndEnabled);
				if (autoDisable) {
					gameObject.SetActive(active);
				} else {
					enabled = active;
					if (camera) camera.enabled = active;
				}
			}
			if (doValueCheck && !doTextureReset) {
				doValueCheck = false;
				if (!rt) doTextureReset = true;
				else if (new Vector2Int(rt.width, rt.height) != targetRes) doTextureReset = true;
			}
			if (doTextureReset) {
				doTextureReset = false;
				if (rt) {
					rt.Release();
					Destroy(rt);
				}
				rt = CreateTexture(targetRes);
				if (camera) camera.targetTexture = rt;
				if (updateMode == UpdateMode.Initialize) {
					if (camera) camera.Render();
				}

				doSetImageValues = true;
			}
			if (doSetImageValues) {
				doSetImageValues = false;

				var ratio = rt.texelSize.y / rt.texelSize.x;

				foreach (var renderImage in renderImages) {
					renderImage.texture = rt;
					var adjustment = ratio > renderImage.ratio
						? (1 - (renderImage.ratio / ratio))
						: (1 - (ratio / renderImage.ratio));
					renderImage.uvRect = new Rect(
						x: adjustment / 2f,
						y: 0,
						width: 1 - adjustment,
						height: 1
					);
				}
			}
		}

		protected void OnDestroy() {
			if (RenderObjects.instance) {
				RenderObjects.instance.Cleanup(this);
			}
			if (rt) {
				rt.Release();
				Destroy(rt);
			}
			Destroy(gameObject);
		}


		internal void RemoveDependent(RenderImage renderImage) {
			renderImages.Remove(renderImage);
			if (!renderImages.Any()) {
				if (prefab.poolSize != 0) {
					if (RenderObjects.instance.Pool(this)) {
						if (autoDisable) gameObject.SetActive(false);
						enabled = camera.enabled = false;
					} else {
						// Pool failed (pool size limit)
						Destroy(gameObject);
					}
				} else {
					Destroy(gameObject);
				}
			}
		}

		internal void AddDependent(RenderImage renderImage) {
			renderImages.Add(renderImage);
			doValueCheck = true;
		}

		internal void OnPickedFromPool() {
			enabled = camera.enabled = true;
		}


		protected virtual RenderTexture CreateTexture(Vector2Int resolution) {
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

#if UNITY_EDITOR
		internal void FixIssues() {
			if (!camera) return;
			var additional = camera.GetUniversalAdditionalCameraData();
			additional.requiresColorOption = CameraOverrideOption.Off;
			additional.requiresDepthOption = CameraOverrideOption.Off;
		}
#endif

		public enum UpdateMode {
			[Tooltip("RenderTexture will be updated every frame.")]
			Always,
			[Tooltip("RenderTexture will be updated when The " + nameof(RenderImage) + " is created or the dimensions change.")]
			Initialize,
			[Tooltip("RenderTexture will not be updated automatically. Call camera.Render to update the texture.")]
			Manual,
		}

		public enum DebthBits {
			None = 0,
			Bits16 = 16,
			Bits24Stencil = 24,
			Bits32Stencil = 32
		}

		public enum Antialiasing {
			None = 1,
			AA2 = 2,
			AA4 = 4,
			AA8 = 8
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
	using UnityEngine.Rendering.Universal;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(RenderObject), true)]
	public class RenderObjectEditor : Editor {

		new RenderObject target => (RenderObject)base.target;
		new IEnumerable<RenderObject> targets => base.targets.Cast<RenderObject>();

		SerializedProperty prefab;

		SerializedProperty _updateMode;
		SerializedProperty _camera;

		SerializedProperty autoDisable;
		SerializedProperty poolSize;
		SerializedProperty shared;

		SerializedProperty _renderScale;
		SerializedProperty _antialiasing;
		SerializedProperty _format;
		SerializedProperty _debthBits;
		SerializedProperty _enableMipMaps;
		SerializedProperty _autoGenerateMips;
		SerializedProperty _dynamicScaling;
		SerializedProperty _filterMode;
		SerializedProperty _anisoLevel;

		protected virtual void OnEnable() {
			prefab = serializedObject.FindProperty(nameof(RenderObject.prefab));

			_updateMode = serializedObject.FindProperty(nameof(_updateMode));
			_camera = serializedObject.FindProperty(nameof(_camera));

			autoDisable = serializedObject.FindProperty(GetBackingFieldName(nameof(RenderObject.autoDisable)));
			poolSize = serializedObject.FindProperty(GetBackingFieldName(nameof(RenderObject.poolSize)));
			shared = serializedObject.FindProperty(GetBackingFieldName(nameof(RenderObject.shared)));

			_renderScale = serializedObject.FindProperty(nameof(_renderScale));
			_antialiasing = serializedObject.FindProperty(nameof(_antialiasing));
			_format = serializedObject.FindProperty(nameof(_format));
			_debthBits = serializedObject.FindProperty(nameof(_debthBits));
			_enableMipMaps = serializedObject.FindProperty(nameof(_enableMipMaps));
			_autoGenerateMips = serializedObject.FindProperty(nameof(_autoGenerateMips));
			_dynamicScaling = serializedObject.FindProperty(nameof(_dynamicScaling));
			_filterMode = serializedObject.FindProperty(nameof(_filterMode));
			_anisoLevel = serializedObject.FindProperty(nameof(_anisoLevel));
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			bool showFix = false;
			if (targets.Any(v => v.camera && v.camera.GetUniversalAdditionalCameraData().requiresColorOption != CameraOverrideOption.Off)) {
				if (serializedObject.isEditingMultipleObjects) HelpBoxField("One of the Cameras may copy the \"Opaque Texture\", which may seriously degrade performance.", MessageType.Warning);
				else HelpBoxField("The Camera may copy the \"Opaque Texture\", which may seriously degrade performance.", MessageType.Warning);
				showFix = true;
			}
			if (targets.Any(v => v.camera && v.camera.GetUniversalAdditionalCameraData().requiresDepthOption != CameraOverrideOption.Off)) {
				if (serializedObject.isEditingMultipleObjects) HelpBoxField("One of the Cameras may copy the \"Depth Texture\", which may seriously degrade performance.", MessageType.Warning);
				else HelpBoxField("The Camera may copy the \"Depth Texture\", which may seriously degrade performance.", MessageType.Warning);
				showFix = true;
			}
			if (showFix) {
				if (ButtonField(new("Fix issues"))) {
					foreach (var target in targets) {
						target.FixIssues();
					}
				}
				Space();
			}

			if (prefab.objectReferenceValue) {
				using (DisabledScope(!Application.isPlaying)) {
					PropertyField(prefab);
				}
			}

			EditorGUI.BeginChangeCheck();

			PropertyField(_updateMode);
			PropertyField(_camera);

			using (DisabledScope(Application.isPlaying)) {
				PropertyField(autoDisable);
				PropertyField(poolSize);
				PropertyField(shared);
			}

			PropertyField(_renderScale);
			PropertyField(_antialiasing);
			PropertyField(_format);
			PropertyField(_debthBits);
			PropertyField(_enableMipMaps);
			using (DisabledScope(!_enableMipMaps.boolValue)) PropertyField(_autoGenerateMips);
			PropertyField(_dynamicScaling);
			PropertyField(_filterMode);
			using (DisabledScope(_debthBits.intValue != 0)) PropertyField(_anisoLevel);

			if (EditorGUI.EndChangeCheck()) {
				foreach (var target in targets) {
					target.doTextureReset = true;
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

	}

}
#endif
