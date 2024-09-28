
namespace Unitylity.Systems.Popups {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using Unitylity.Components.Extended;
	using Object = UnityEngine.Object;

#if UNITYLITY_SYSTEMS_POPUPS_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Popups) + "/" + nameof(Popups))]
#endif
	[RequireComponent(typeof(RectTransform))]
	public class Popups : UISingleton<Popups> {

		[field: SerializeField, Tooltip("Used as the default popup.")]
		public PopupPreset defaultPopup { get; private set; }

		[HideInInspector] public List<Popup> popups = new();

		/// <summary> Shows the default messagebox </summary>
		public static Popup ShowPopup(string title, string message) {
			var res = Popups.instance.defaultPopup.Show(title, message);
			return res;
		}

		public static void TryClose() {
			if (instance.popups.Count > 0) {
				var popup = instance.popups.Last();
				var option = popup.options.FirstOrDefault(v => v.button.interactable && v.button.isActiveAndEnabled && v.isActiveAndEnabled && v.flags.HasFlag(PopupOption.Flags.Cancel));
				if (option) option.Invoke();
			}
		}

	}

}