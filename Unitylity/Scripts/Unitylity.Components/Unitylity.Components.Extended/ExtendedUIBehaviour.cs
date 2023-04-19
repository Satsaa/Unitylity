
namespace Unitylity.Components.Extended {

	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using UnityEngine.EventSystems;

	public abstract class ExtendedUIBehaviour : UIBehaviour {

		RectTransform _rectTransform;
		bool _rectTransformChecked;
		public RectTransform rectTransform => (_rectTransformChecked == (_rectTransformChecked = true)) ? _rectTransform : _rectTransform = GetComponent<RectTransform>();

	}

}