

namespace Muc.Systems.RenderImages {
	using System;
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
	[AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.RenderImages) + "/" + nameof(RenderObject))]
#endif
	[RequireComponent(typeof(Camera))]
	public class RenderObject : MonoBehaviour {

		[SerializeField] private UpdateMode _updateMode = UpdateMode.Initialize;
		protected UpdateMode updateMode {
			get => _updateMode;
			set {
				if (_updateMode == value) return;
				_updateMode = value;
				camera.enabled = _updateMode == UpdateMode.Always;
			}
		}

		private Camera _camera;
		new protected Camera camera {
			get {
				if (_camera == null) _camera = GetComponent<Camera>();
				return _camera;
			}
		}

		[SerializeField] public Transform renderRoot;
		[SerializeField, HideInInspector] protected RenderTexture rt;

		[field: SerializeField, HideInInspector] public List<RenderImage> renderImages { get; private set; } = new List<RenderImage>();
		public RenderImage driver => renderImages.First();
		public Vector2Int targetRes => renderImages.Aggregate(driver.rawResolution, (acc, v) => !v.enabled ? acc : acc.Max(v.rawResolution)).Mul(driver.renderScale).RoundInt().Max(1);

		protected internal bool doCheckEnable;
		protected internal bool doValueCheck;
		protected bool doTextureReset;


		protected void Awake() {
			camera.enabled = false;
		}

		protected void Start() {
			camera.enabled = updateMode == UpdateMode.Always;
			Debug.Assert(renderRoot, this);
			if (updateMode == UpdateMode.Initialize) {
				camera.Render();
			}
		}

		protected void LateUpdate() {
			if (doCheckEnable) {
				doCheckEnable = false;
				var active = renderImages.Any(v => v.isActiveAndEnabled);
				gameObject.SetActive(active);
			}
			if (doValueCheck) {
				doValueCheck = false;
				if (!rt) doTextureReset = true;
				else if (rt.texelSize != targetRes) doTextureReset = true;
				else if (rt.useMipMap != driver.enableMipMaps) doTextureReset = true;
				else if (rt.autoGenerateMips != (driver.autoGenerateMips && driver.enableMipMaps)) doTextureReset = true;
				else if (rt.antiAliasing != (int)driver.antialiasing) doTextureReset = true;
				else if (rt.useDynamicScale != driver.dynamicScaling) doTextureReset = true;
				else if (rt.filterMode != driver.filterMode) doTextureReset = true;
				else if (rt.anisoLevel != (driver.debthBits == RenderImage.DebthBits.None ? driver.anisoLevel : 0)) doTextureReset = true;
			}
			if (doTextureReset) {
				doTextureReset = false;
				rt = driver.CreateTexture(targetRes);
				camera.targetTexture = rt;
				if (updateMode == UpdateMode.Initialize) {
					camera.Render();
				}
				foreach (var renderImage in renderImages) renderImage.texture = rt;
			}
		}

		protected void OnDestroy() {
			Destroy(gameObject);
		}


		internal void RemoveDependent(RenderImage renderImage) {
			renderImages.Remove(renderImage);
			if (!renderImages.Any()) {
				Destroy(gameObject);
			}
		}

		internal void AddDependent(RenderImage renderImage) {
			renderImages.Add(renderImage);
			doValueCheck = true;
		}


		protected enum UpdateMode {
			Always,
			Initialize,
			Never,
		}

	}

}