

namespace Muc.Components.Extended {

	using System.Linq;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;

	public abstract class ExtendedUIBehaviour : UIBehaviour {

		RectTransform _rectTransform;
		public RectTransform rectTransform {
			get {
				if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
				return _rectTransform;
			}
		}

	}

}