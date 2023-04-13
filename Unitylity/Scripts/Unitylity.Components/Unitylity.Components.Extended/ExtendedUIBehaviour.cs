
namespace Unitylity.Components.Extended {

	using System.Collections.Generic;
	using System.Linq;
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