
namespace Unitylity.Components.Extended {

	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using UnityEngine.EventSystems;

	public abstract class ExtendedUIBehaviour : UIBehaviour {

		RectTransform _rectTransform;
		public RectTransform rectTransform => _rectTransform == null ? _rectTransform = GetComponent<RectTransform>() : _rectTransform;

#if UNITY_EDITOR
		// WHY DID THEY DO THIS?
		protected override void OnValidate() => base.OnValidate();
		protected override void Reset() => base.Reset();
#else
        protected virtual void OnValidate() {}
        protected virtual void Reset() {}
#endif
	}

}