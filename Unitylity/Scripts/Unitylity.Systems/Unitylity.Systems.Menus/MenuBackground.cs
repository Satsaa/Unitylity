
namespace Unitylity.Systems.Menus
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;
	using Unitylity.Data;
	using Object = UnityEngine.Object;

	[AddComponentMenu("")]
	public sealed class MenuBackground : RawImage, IPointerClickHandler
	{
		[field: SerializeField] public Color toColor { get; internal set; }
		[field: SerializeField] public Color fromColor { get; internal set; }
		[field: SerializeField] new public float animation { get; internal set; }
		[field: SerializeField] public bool destroy { get; internal set; }
		[field: SerializeField] public Menu menu { get; internal set; }

		public void OnPointerClick(PointerEventData eventData)
		{
			if (menu.bgPressHidesMenu)
			{
				menu.Hide();
			}
		}
	}

}
