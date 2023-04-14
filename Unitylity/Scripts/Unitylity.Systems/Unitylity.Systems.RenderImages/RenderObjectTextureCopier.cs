
namespace Unitylity.Systems.RenderImages {

	using System;
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
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.RenderImages) + "/" + nameof(RenderObjectTextureCopier))]
#endif
	[RequireComponent(typeof(MeshRenderer))]
	public class RenderObjectTextureCopier : MonoBehaviour {

		private RenderTexture tex;

		void Update() {
			var mr = GetComponent<MeshRenderer>();
			var cam = GetComponentInParent<Camera>();
			if (!cam) return;
			var rt = cam.targetTexture as RenderTexture;
			if (!rt) return;
			if (tex == null || rt.width != tex.width || rt.height != tex.height) {
				tex = new RenderTexture(rt);
				mr.material.SetTexture("_OtherTex", tex);
			}
			Graphics.Blit(rt, tex);
		}

	}

}