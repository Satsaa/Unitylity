
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
	[AddComponentMenu("Unitylity/" + nameof(Muc.Systems.RenderImages) + "/" + nameof(RenderObjectTextureCopier))]
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